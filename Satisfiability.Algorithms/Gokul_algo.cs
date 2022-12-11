/*
    Acknowledgements:
    --clearly state which existing algorithms you are improving upon (if any)--
*/
using System;
using System.Collections.Generic;

namespace Satisfiability.Algorithms
{
    public class Gokul_algo : Abstract
    {
        public Gokul_algo(
            int seed,
            Action<int> writeAlgoIdentifier,
            Func<List<bool>, bool> isInputSolution,
            bool debugMode
        ) : base(seed, writeAlgoIdentifier, isInputSolution, debugMode)
        {
        }
        public override List<bool> Solve(int numVariables, List<List<int>> clauses)
        {

            static object get_any_clause_as_bool(List<bool> sols, List<int> clause)
            {
                var clause_as_bool = new List<bool>();
                foreach (var idx in clause)
                {
                    if (idx > 0)
                    {
                        clause_as_bool = new List<bool>();
                        clause_as_bool.Add(sols[idx - 1]);
                    }
                    else if (idx < -1)
                    {
                        clause_as_bool = new List<bool>();
                        clause_as_bool.Add(!sols[Math.Abs(idx) + 1]);
                    }
                    else
                    {
                        clause_as_bool = new List<bool>();
                        clause_as_bool.Add(!sols[0]);
                    }
                }
                return !clause_as_bool.TrueForAll(x => x == false);
            }

            public static object sols = new List<object> {
                    true
                } * numVariables;

        public static object counter = 0;

        public static object counter = 1;

        static Module()
        {
            sols[idx] = !sols[idx];
            sols[idx] = !sols[idx];
        }

            if (DebugMode)
                Debug.Log("Hello world!");
            return new ();
        }
}
}
