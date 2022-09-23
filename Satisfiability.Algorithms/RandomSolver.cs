using System;
using System.Collections.Generic;
using System.Linq;

namespace Satisfiability.Algorithms
{
    public class RandomSolver : Abstract
    {
        public RandomSolver(
            int seed,
            Action<int> writeAlgoIdentifier,
            Func<List<bool>, bool> isInputSolution,
            bool debugMode
        ) : base(seed, writeAlgoIdentifier, isInputSolution, debugMode)
        {
        }
        public override List<bool> Solve(int numVariables, List<List<int>> clauses)
        {
            for (int attempt = 1; attempt <= 1000; attempt++)
            {
                if (DebugMode && attempt % 200 == 0)
                    Debug.Log($"Number of Attempts: {attempt}");

                List<bool> input = new List<bool>();
                int numVariablesRemaining = numVariables;
                while (numVariablesRemaining != 0)
                {
                    int n = Math.Min(numVariablesRemaining, 30);
                    int nBits = Random.Next(1 << n);
                    for (int i = 0; i < n; i++)
                        input.Add((nBits & (1 << i)) != 0);
                    numVariablesRemaining -= n;
                }

                // generate and write an int that cannot be guessed without running your algorithm
                WriteAlgoIdentifier(Random.Next() * input.LastIndexOf(true) * input.LastIndexOf(false));
                if (IsInputSolution(input))
                    return input;
            }
            return new();
        }
    }
}
