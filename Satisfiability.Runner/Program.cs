using Satisfiability.Challenge;

namespace Satisfiability.Runner
{
    public class Program
    {
        static Random Random = new Random(1337);
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                args = new string[] {
                    $"collegeTry", // typeof(Algorithms.RandomSolver).Name
                    $"{true}",
                    $"{int.MaxValue}"
                };
            }

            Type algorithm = Utils.GetAlgorithm(args[0]);
            bool debug = bool.Parse(args[1]);
            int numRuns = int.Parse(args[2]);

            for (int i = 0; i < numRuns; i++)
            {
                Run(algorithm, debug);
            }
        }
        public static void Run(Type algorithm, bool debug = false)
        {
            var start = DateTime.Now;
            var difficulty = new Difficulty(
                // the number of variables in the 3SAT boolean formula
                numVariables: Random.Next(20, 50),

                // the ratio of num clauses to num variables in the 3SAT boolean formula
                // clausesToVariablesRatio is a fixed point number with 1/100 scaling. i.e. 3.60 is stored as 360
                clausesToVariablesRatio: Random.Next(100, 200)
            );
            int seed = Random.Next();
            var challenge = difficulty.GenerateChallenge(seed);
            var solveResult = challenge.Solve(algorithm, debugMode: debug);

            Console.WriteLine($"Satisfiability, {algorithm.Name}, {seed}, {difficulty.NumVariables}, {difficulty.ClausesToVariablesRatio}, {solveResult.IsSolution}, {(DateTime.Now - start).TotalSeconds}");
        }
    }
}
