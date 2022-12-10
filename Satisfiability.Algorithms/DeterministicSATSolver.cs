/*
   Deterministc SAT solver moves according to an ordered counter, https://www.nature.com/articles/s41598-020-76666-2
  
*/
using System;
using System.Linq;
using System.Collections.Generic;

namespace Satisfiability.Algorithms
{
    public class DeterministicSATSolver : Abstract
    {
        public DeterministicSATSolver(
            int seed,
            Action<int> writeAlgoIdentifier,
            Func<List<bool>, bool> isInputSolution,
            bool debugMode
        ) : base(seed, writeAlgoIdentifier, isInputSolution, debugMode)
        {
        }
        public override List<bool> Solve(int numVariables, List<List<int>> clauses)
        {

                var input = Enumerable.Repeat(true, numVariables).ToList();

            while (clauses.Count > 0)
            {

                var Counter = new Dictionary<int, int>();
                var CounterNegative = new Dictionary<int, int>();
                var CounterIndex = new Dictionary<int, List<int>>();
                var CounterIndexNegative = new Dictionary<int, List<int>>();
                for (int i = 1; i <= numVariables; i++)
                {
                    Counter[i] = 0;
                    CounterNegative[i] = 0;
                    CounterIndex[i] = new List<int>();
                    CounterIndexNegative[i] = new List<int>();
                }
                var flatClauses = clauses.SelectMany(i => i);

                int index = 0;
                foreach (var num in flatClauses)
                {
                    Counter[Math.Abs(num)] += 1;
                    CounterIndex[Math.Abs(num)].Add(index);
                    index++;
                    if (num < 0)
                    {
                        CounterNegative[Math.Abs(num)] += 1;
                        CounterIndexNegative[Math.Abs(num)].Add(index);
                    }


                }
                var sortedCounter = from entry in Counter orderby entry.Value descending select entry;

                int maxOccuredValue = sortedCounter.First().Key;
                int clauseLength = clauses[0].Count;

                bool inputValue = ((double)CounterNegative[maxOccuredValue] / (double)Counter[maxOccuredValue] < 0.5);
                input[maxOccuredValue-1] = inputValue;

                foreach (var maxOccurValueIdx in CounterIndex[maxOccuredValue].Reverse<int>())
                {
                    int clauseIdx = maxOccurValueIdx / clauseLength;
                    if ((inputValue && flatClauses.ElementAt(maxOccurValueIdx) > 0) ||
                        (!inputValue && flatClauses.ElementAt(maxOccurValueIdx) < 0))
                    {
                        clauses.RemoveAt(clauseIdx);
                    }
                }



                // generate and write a unique integer that identifies when someone is using your algorithm
                int uniqueInt = 1;
                for (int i = 1; i < input.Count; i++)
                    uniqueInt *= input[i] ? i : 1;
                WriteAlgoIdentifier(uniqueInt);

                if (IsInputSolution(input))
                    return input;
            }
            return new();
        }
    }
}
