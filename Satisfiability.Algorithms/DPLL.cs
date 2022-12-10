/*
    Acknowledgements:
    --This is an implementation of the Davis–Putnam–Logemann–Loveland (DPLL) algorithm (https://en.wikipedia.org/wiki/DPLL_algorithm)
    --The idea of using dictionnaries is from Jack's Satv1  
*/
using System;
using System.Collections.Generic;

namespace Satisfiability.Algorithms
{

    public class DPLL : Abstract
    {
        public DPLL(
            int seed,
            Action<int> writeAlgoIdentifier,
            Func<List<bool>, bool> isInputSolution,
            bool debugMode
        ) : base(seed, writeAlgoIdentifier, isInputSolution, debugMode)
        {
        }

        private bool containsUnitClause(List<List<int>> clauses) {
            foreach (List<int> clause in clauses){
                if (clause.Count == 1){
                    return true;
                }
            }
            return false;
        }


        public override List<bool> Solve(int numVariables, List<List<int>> clauses)
        {
            List<bool> input = new List<bool>();
        
            var assignments = DPLL(clauses, new(), numVariables);

            for (int i = 1; i <= numVariables; i++){
                if (assignments.ContainsKey(i)){
                    input.Add(assignments[i]);
                }
                else {
                    input.Add(true);
                }
            }

            return input;
        }

        private void printList(List<List<int>> clauses){
            foreach (List<int> clause in clauses){
                    Console.WriteLine(String.Join(", ", clause));
                }
            Console.WriteLine("");
        }

        private void printDictionnary(Dictionary<int, bool> dictionnary){
            foreach (int i in dictionnary.Keys){
                Console.WriteLine("{0}:  {1}", i, dictionnary[i]);
            }
        }

        private Dictionary<int, bool> DPLL (List<List<int>> clauses, Dictionary<int,bool> assignments, int numVariables){
            assignments = removeUnitClauses(clauses, assignments);
            assignments = propagatePureLiterals(clauses, assignments);
            if (clauses.Count == 0){
                return assignments;
            }
            if (containsEmptyClause(clauses)){
                return new();
            }

            var firstNewList = deepCopyClauses(clauses);
            var secondNewList = deepCopyClauses(clauses);
            var firstNewDict = new Dictionary<int,bool>(assignments);
            var secondNewDict = new Dictionary<int,bool>(assignments);
            for (int i = 1; i <= numVariables; i++){
                if (!assignments.ContainsKey(i)){
                    propagateAssignment(i, firstNewList);
                    propagateAssignment(-i, secondNewList);
                    firstNewDict.Add(i, true);
                    secondNewDict.Add(i, false);
                    break;
                }
            }
            if (DPLL(firstNewList, firstNewDict, numVariables).Count != 0){
                return DPLL(firstNewList, firstNewDict, numVariables);
            }
            if (DPLL(secondNewList, secondNewDict, numVariables).Count != 0){
                return DPLL(secondNewList, secondNewDict, numVariables);
            }
            return assignments;
        }

        private Dictionary<int,bool> removeUnitClauses(List<List<int>> clauses, Dictionary<int,bool> assignments){
            while(containsUnitClause(clauses)){
                for (int index = clauses.Count - 1; index >= 0; index--){
                    var clause = clauses[index];
                    if (clause.Count == 1){
                        if (!assignments.ContainsKey(Math.Abs(clause[0]))){
                            assignments.Add(Math.Abs(clause[0]), true ? clause[0] > 0 : false);
                        }
                        clauses.RemoveAt(index);
                        propagateAssignment(clause[0], clauses);
                        break;
                    }
                }
            }
            return assignments;
        }

        private Dictionary<int,bool> propagatePureLiterals(List<List<int>> clauses, Dictionary<int,bool> assignments){
            var pureLiterals = getPureLiterals(clauses);
            bool removeClause = false;

            for (int index = clauses.Count - 1; index >= 0; index--){
                var clause = clauses[index];
                removeClause = false;
                foreach (int i in clause){
                    if (pureLiterals.Contains(i)){
                        if (!assignments.ContainsKey(Math.Abs(i))){
                            assignments.Add(Math.Abs(i), true ? i > 0 : false);
                        }
                        removeClause = true;
                    }
                }
                if (removeClause == true){
                    clauses.RemoveAt(index);
                }
            }
            return assignments;
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

        private bool containsEmptyClause(List<List<int>> clauses){
            foreach(var clause in clauses){
                if (clause.Count == 0){
                    return true;
                }
            }
            return false;
        }

        private void propagateAssignment(int variable, List<List<int>> clauses){
            for (int index = clauses.Count - 1; index >= 0; index--){
                var clause = clauses[index];
                if (clause.Contains(variable)){
                    // if (variable == -1){
                        // Console.WriteLine("S");
                    // }
                    clauses.RemoveAt(index);
                }
                else if (clause.Contains(-variable)){
                    clauses[index].Remove(-variable);
                }
            }
        }
    
        private List<List<int>> deepCopyClauses(List<List<int>> clauses){
            var newClauses = new List<List<int>>(clauses.Count);

            foreach (var clause in clauses){
                List<int> newClause = new();
                foreach (int i in clause){
                    newClause.Add(i);
                }
                newClauses.Add(newClause);
            }

            return newClauses;
        }

    }
}
