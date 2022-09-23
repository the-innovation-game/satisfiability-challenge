using System;
using System.Collections.Generic;

namespace Satisfiability.Algorithms
{
    public abstract class Abstract
    {
        public Abstract(
            int seed,
            Action<int> writeAlgoIdentifier,
            Func<List<bool>, bool> isInputSolution,
            bool debugMode
        )
        {
            DebugMode = debugMode;
            Random = new Random(seed);
            WriteAlgoIdentifier = writeAlgoIdentifier;
            IsInputSolution = isInputSolution;
        }
        public readonly bool DebugMode;
        public readonly Random Random;

        // invoke this every so often with an int that cannot be guessed without running your algo
        public readonly Action<int> WriteAlgoIdentifier;
        // helper func to check whether your input is a solution
        public readonly Func<List<bool>, bool> IsInputSolution;
        public abstract List<bool> Solve(int numVariables, List<List<int>> clauses);
    }
    public class Debug
    {
        private static void _Log(object message, string level) =>
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff")} - [{level}] - {message}");
        public static void Log(object message) => _Log(message, "DEBUG");
        public static void LogError(object message) => _Log(message, "ERROR");
        public static void LogWarning(object message) => _Log(message, "WARNING");
        public static void LogException(object message) => _Log(message, "EXCEPTION");
    }
}
