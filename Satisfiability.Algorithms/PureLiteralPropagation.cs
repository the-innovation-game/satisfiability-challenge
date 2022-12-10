/*
    Acknowledgements:
    --Ideas and terminology taken from these slides: https://sat.inesc-id.pt/~ines/sac10.pdf
    --This algorithm first identifies pure literals in the clauses and then randomly samples for all remaining variables
    --Some of the random logic has been taken from RandomSolver  
*/
using System;
using System.Collections.Generic;

namespace Satisfiability.Algorithms
{
    public class PureLiteralPropagation : Abstract
    {
        public PureLiteralPropagation(
            int seed,
            Action<int> writeAlgoIdentifier,
            Func<List<bool>, bool> isInputSolution,
            bool debugMode
        ) : base(seed, writeAlgoIdentifier, isInputSolution, debugMode)
        {
        }

        private HashSet<int> getPureLiterals(List<List<int>> clauses){
            var variablesInClauses = getVariablesInClauses(clauses);
            var pureLiterals = new HashSet<int>(); 
            foreach (int variable in variablesInClauses){
                if (!variablesInClauses.Contains(-variable)){
                    pureLiterals.Add(variable);
                }
            }
            return pureLiterals;
        }

        private HashSet<int> getVariablesInClauses(List<List<int>> clauses){
            var uniqueVariables = new HashSet<int>();
            foreach (var clause in clauses){
                foreach(var i in clause){
                    uniqueVariables.Add(i);
                }
            }
            return uniqueVariables;
        }

        public override List<bool> Solve(int numVariables, List<List<int>> clauses)
        {
            // Find pure literals
            var pureLiteralsd = getPureLiterals(clauses);

            for (int attempt = 1; attempt <= 1000; attempt++){

                List<bool> input = new List<bool>();
                
                // Iterate over all variables 
                for (int variable = 1; variable <= numVariables; variable++){

                    // If the variable is a pure literal, fix its value accordingly
                    if (pureLiteralsd.Contains(variable)){
                        input.Add(true);
                    }
                    else if (pureLiteralsd.Contains(-variable)){
                        input.Add(false);
                    }
                    else // Otherwise set it randomly
                    {
                        input.Add(Random.NextDouble() > 0.5);
                    }

                }

                 // generate and write a unique integer that identifies when someone is using your algorithm
                int uniqueInt = 1;
                for (int i = 1; i < input.Count; i++)
                    uniqueInt *= input[i] ? i : 1;
                WriteAlgoIdentifier(uniqueInt);

                // check if solution has been found
                if (IsInputSolution(input))
                    return input;
            }


            return new();
        }
    }
}
