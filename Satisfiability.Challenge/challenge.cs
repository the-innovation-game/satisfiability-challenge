using System;
using System.Numerics;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Satisfiability.Challenge
{
    public class MismatchedAlgorithm : Exception
    {
        public MismatchedAlgorithm(string message) : base(message) { }
    }
    public class Difficulty
    {
        public int NumVariables { get; init; } // number of variables from which 3 literals are sampled
        public int ClausesToVariablesRatio { get; init; } // fixed point number with 1/100 scaling. i.e. 3.53 is stored as 353
        public int NumClauses => (int)(NumVariables * ClausesToVariablesRatio / 100);
        public int NumLiteralsPerClause => 3; // hardcoded to 3SAT

        public static Difficulty FromEncoding(string[] fields)
        {
            return new Difficulty(
                    numVariables: int.Parse(fields[0]),
                    clausesToVariablesRatio: int.Parse(fields[1])
                );
        }

        public Difficulty(int numVariables, int clausesToVariablesRatio)
        {
            if (numVariables < 3)
                throw new ArgumentException("Value for Satisfiability.Difficulty.NumVariables must be >= 3");
            if (clausesToVariablesRatio < 1)
                throw new ArgumentException("Value for Satisfiability.Difficulty.ClausesToVariablesRatio must be >= 1");
            NumVariables = numVariables;
            ClausesToVariablesRatio = clausesToVariablesRatio;
        }

        public string[] Fields()
        {
            return new[] {
                NumVariables.ToString(),
                ClausesToVariablesRatio.ToString(),
            };
        }

        public Challenge GenerateChallenge(int seed)
        {
            return new Challenge(this, seed);
        }
    }

    public partial class Solution
    {
        public List<bool> Input { get; init; }
        public byte[] StateUpdates { get; init; }

        public Solution(List<bool> input, byte[] stateUpdates)
        {
            Input = input;
            StateUpdates = stateUpdates;
        }
        protected void Write(BinaryWriter writer)
        {
            writer.Write(Input.Count);
            foreach (var v in Input)
                writer.Write(v);
            writer.Write(StateUpdates.Length);
            writer.Write(StateUpdates);
        }

        public bool VerifySolutionOnly(Challenge challenge, double maxSecondsTaken = double.MaxValue)
        {
            return challenge.IsInputSolution(Input);
        }

        public bool VerifyMethodOnly(Challenge challenge, Type algorithm, double maxSecondsTaken = double.MaxValue)
        {
            DateTime start = DateTime.Now;

            using (Stream updatesStream = new MemoryStream(StateUpdates))
            using (BinaryReader reader = new BinaryReader(updatesStream))
            {
                var checkAlgoIdentifier = delegate (int update)
                {
                    if (Utils.CalcRemainingMaxSecondsTaken(start, maxSecondsTaken) <= 0)
                        throw new TimeoutException($"Solving challenge exceeded maxSecondsTaken '{maxSecondsTaken}'");
                    if (reader.ReadInt32() != update)
                        throw new MismatchedAlgorithm("Mismatched algo");
                };

                try
                {
                    var solver = (Algorithms.Abstract)Activator.CreateInstance(algorithm, challenge.Seed, checkAlgoIdentifier, (object)challenge.IsInputSolution, false);

                    solver.Solve(
                        numVariables: challenge.NumVariables,
                        clauses: challenge.Clauses.Select(c => c.ToList()).ToList()
                    );

                    return updatesStream.Position == updatesStream.Length;
                }
                catch (MismatchedAlgorithm)
                {
                    return false;
                }
            }
        }

        public static Solution Read(BinaryReader reader)
        {
            List<bool> input = new();
            var numVariables = reader.ReadInt32();
            for (int i = 0; i < numVariables; i++)
                input.Add(reader.ReadBoolean());
            return new Solution(
                input: input,
                stateUpdates: reader.ReadBytes(reader.ReadInt32())
            );
        }
    }
    public class Challenge
    {
        public int Seed { get; init; }
        public Difficulty Difficulty { get; init; }
        public int NumVariables => Difficulty.NumVariables;
        public int NumClauses => Difficulty.NumClauses;
        public int NumLiteralsPerClause => Difficulty.NumLiteralsPerClause;
        public List<List<int>> Clauses { get; init; }

        public Challenge(Difficulty difficulty, int seed)
        {
            Difficulty = difficulty;
            Seed = seed;
            Random random = new Random(seed + 1337);

            Clauses = new List<List<int>>();
            var variables = Enumerable.Range(1, difficulty.NumVariables).ToList();
            for (int i = 0; i < difficulty.NumClauses; i++)
            {
                var clause = new List<int>();
                for (int j = 0; j < difficulty.NumLiteralsPerClause; j++)
                {
                    int idx = random.Next(difficulty.NumVariables - j);
                    int v = variables[idx];
                    (variables[idx], variables[difficulty.NumVariables - 1 - j]) = (variables[difficulty.NumVariables - 1 - j], variables[idx]);

                    if (random.Next() % 2 == 0)
                        v = -v;

                    clause.Add(v);
                }
                Clauses.Add(clause);
            }
        }

        public SolveResult Solve(Type algorithm, double maxSecondsTaken = double.MaxValue, bool debugMode = false)
        {
            DateTime start = DateTime.Now;

            using (MemoryStream updatesStream = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(updatesStream))
            {
                var writeAlgoIdentifier = delegate (int update)
                {
                    if (Utils.CalcRemainingMaxSecondsTaken(start, maxSecondsTaken) <= 0)
                        throw new TimeoutException($"Solving challenge exceeded maxSecondsTaken '{maxSecondsTaken}'");
                    writer.Write(update);
                };

                var solver = (Algorithms.Abstract)Activator.CreateInstance(algorithm, Seed, writeAlgoIdentifier, (object)IsInputSolution, debugMode);

                List<bool> input = solver.Solve(
                    numVariables: NumVariables,
                    clauses: Clauses.Select(c => c.ToList()).ToList()
                );

                return new SolveResult()
                {
                    IsSolution = IsInputSolution(input),
                    Solution = new Solution(input, updatesStream.ToArray())
                };
            }
        }
        public bool IsInputSolution(List<bool> input)
        {
            if (input.Count != Difficulty.NumVariables)
                return false;

            return Clauses.All(clause => clause.Any(
                i => i < 0 ? !input[Math.Abs(i) - 1] : input[Math.Abs(i) - 1]
            ));
        }
    }
}
