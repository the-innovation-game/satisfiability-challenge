using System;
using System.Collections.Generic;
using System.Linq;

namespace Satisfiability.Algorithms
{
    public class FastSat : Abstract
    {
        public FastSat(
            int seed,
            Action<int> writeAlgoIdentifier,
            Func<List<bool>, bool> isInputSolution,
            bool debugMode
        ) : base(seed, writeAlgoIdentifier, isInputSolution, debugMode)
        {
        }

        private HashSet<int> findUniqueLiterals(List<List<int>> clauses)
        {
            HashSet<int> literals = new HashSet<int>();
            foreach (List<int> clause in clauses)
            {
                foreach (int num in clause)
                {
                    literals.Add(num);
                }
            }
            return literals;
        }

        private (HashSet<int>, bool) deduceAssignments(List<List<int>> clauses, HashSet<int> assignments)
        {
            bool foundNewAssignments = false;

            var literals = findUniqueLiterals(clauses);

            foreach (int lit in literals)
            {
                if (!literals.Contains(lit * -1))
                {
                    if (!assignments.Contains(lit))
                    {
                        assignments.Add(lit);
                        foundNewAssignments = true;
                    }
                }
            }

            foreach (List<int> clause in clauses)
            {
                if (clause.Count == 1)
                {
                    if (assignments.Contains(-clause[0]))
                    {
                        throw new ArgumentException();
                    }
                    if (!assignments.Contains(clause[0]))
                    {
                        assignments.Add(clause[0]);
                        foundNewAssignments = true;
                    }
                }
            }

            return (assignments, foundNewAssignments);
        }

        private List<List<int>> applyAssignments(List<List<int>> clauses, HashSet<int> assignments)
        {
            for (int index = clauses.Count - 1; index >= 0; index--)
            {
                foreach (int value in clauses[index])
                {
                    if (assignments.Contains(value))
                    {
                        clauses.RemoveAt(index);
                        break;
                    }
                }
            }
            return clauses;
        }

        private (bool, HashSet<int>) recursiveGuess(HashSet<int> guessSet, List<List<int>> clauses, HashSet<int> assignments)
        {
            HashSet<int> copyAssignments = new HashSet<int>(assignments);
            List<List<int>> copyClauses = clauses.Select(clause => clause.ToList()).ToList();

            if (isFinished(clauses))
            {
                return (true, copyAssignments);
            }
            if (guessSet.Count == 0)
            {
                return (false, assignments);
            }
            int guess = guessSet.ElementAt(this.Random.Next(guessSet.Count));
            guessSet.Remove(guess);
            bool oppositeRemoved = guessSet.Remove(-guess);
            try
            {
                copyAssignments.Add(guess);
                copyClauses = applyAssignments(copyClauses, copyAssignments);
                (copyAssignments, bool foundNewAssignments) = deduceAssignments(copyClauses, copyAssignments);
                if (foundNewAssignments)
                {
                    copyClauses = applyAssignments(copyClauses, copyAssignments);
                }
                if (isFinished(copyClauses))
                {
                    return (true, copyAssignments);
                }
                return recursiveGuess(guessSet, copyClauses, copyAssignments);
            }
            catch (ArgumentException)
            {
                if (oppositeRemoved)
                {
                    guessSet.Add(-guess);
                }
                return recursiveGuess(guessSet, clauses, assignments);
            }
        }

        public override List<bool> Solve(int numVariables, List<List<int>> clauses)
        {
            /*
                Your Objective:
                     Find a combination of boolean values such that the boolean formula evaluates to true

                Rules:
                     1. You should implement this function
                     2. You are not allowed to use external libraries
                     3. You should invoke WriteAlgoIdentifier every so often with a dynamically generated integer
                        * This allows us to verify when someone is using your algorithm
                        e.g. multiply all non-zero index of True in your current solution attempt
                     4. If you need to generate random numbers, you must use this.Random
                     5. If you want to give up on the challenge (e.g. maybe its unsolvable), you should return new()
                     6. Your algorithm name must be less than or equal to 20 characters (alpha-numeric only)
                     7. Your class name and filename must be `<algorithm_name>.cs` 
                     8. All your utility classes should be nested in this class or contained in a namespace unique to your algorithm
                     9. If you are improving an existing algorithm, make a copy of the code before making modifications

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

            HashSet<int> assignments = new HashSet<int>();
            HashSet<int> uniqueLiterals = findUniqueLiterals(clauses);

            try
            {

                (assignments, bool foundNewAssignments) = deduceAssignments(clauses, assignments);
                if (foundNewAssignments)
                {
                    clauses = applyAssignments(clauses, assignments);
                    if (isFinished(clauses))
                    {
                        return createSolution(assignments, numVariables);
                    }
                }

                HashSet<int> guessSet = new HashSet<int>(uniqueLiterals.Except(assignments));

                (bool done, assignments) = recursiveGuess(guessSet, clauses, assignments);
                if (done)
                {
                    return createSolution(assignments, numVariables);
                }
                else
                {
                    return new();
                }
            }
            catch (ArgumentException)
            {
                return new();
            }
        }

        private bool isFinished(List<List<int>> clauses)
        {
            return clauses.Count == 0;
        }

        private List<bool> createSolution(HashSet<int> assignments, int numVariables)
        {
            List<bool> output = new List<bool>(new bool[numVariables]);
            foreach (int value in assignments.ToList())
            {
                output[Math.Abs(value) - 1] = true ? value > 0 : false;
            }
            return output;
        }
    }
}
