using System;
using System.Collections.Generic;

namespace Satisfiability.Algorithms
{
    public class Template : Abstract
    {
        public Template(
            int seed,
            Action<int> writeAlgoIdentifier,
            Func<List<bool>, bool> isInputSolution,
            bool debugMode
        ) : base(seed, writeAlgoIdentifier, isInputSolution, debugMode)
        {
        }
        public override List<bool> Solve(int numVariables, List<List<int>> clauses)
        {
            /*
                Your Objective:
                     Find a combination of boolean values such that the boolean formula evaluates to true

                Rules:
                     1. You must implement this function
                     2. You are not allowed to use external libraries
                     3. You must invoke WriteAlgoIdentifier every so often with an int that cannot be guessed without running your algorithm
                        e.g. WriteAlgoIdentifier(Random.Next() * input.LastIndexOf(true) * input.LastIndexOf(false));
                     4. If you need to generate random numbers, you must use this.Random
                     5. If you want to give up on the challenge (e.g. maybe its unsolvable), you should return new()
                     6. Your class name and filename must be `<algorithm_name>.cs`
                     7. All your utility classes should be contained in a separate namespace `TheInnovationGame.<challenge_type>.Algorithms.<algorithm_name>Utils`
                     8. If you are improving an existing algorithm, make a copy of the code before making modifications
                     9. Your algorithm name must be less than or equal to 20 characters

                Example Challenge:
                     NumVariables:  5
                     Clauses:
                         {
                             { 2, -3, 1 },
                             { -4, -2, 5 }
                         }

                     The above clauses represent the following boolean formula:
                         (v2 or not v3 or v1) and (not v4 or not v2 or v5)

                Example Solution (there may be more than one solution):
                     Input:
                         { true, false, false, false, false }
                            v1     v2    v3      v4    v5

                Example Solution evaluated against Challenge:
                    (false or not false or true) and (not false or not false or false) = true
             */

            if (DebugMode)
                Debug.Log("Hello world!");
            return new();
        }
    }
}
