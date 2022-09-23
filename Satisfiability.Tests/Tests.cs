using Xunit;
using Satisfiability.Challenge;

namespace Satisfiability.Tests
{
    [Collection("Satisfiability")]
    public class Tests
    {

        [Theory]
        [InlineData("Satisfiability,10:303,Heirloom-5,1,1248,9c21a45927e3b5628c7c320572a648dc,RandomSolver,585294822")]
        public void Solve_Works(string encoding)
        {
            var challengeParams = Params.FromEncoding(encoding);
            challengeParams.Solve(60);
        }

        [Theory]
        [InlineData("Satisfiability,10:303,Heirloom-5,1,1248,9c21a45927e3b5628c7c320572a648dc,RandomSolver,585294822")]
        public void Solution_Encoding_Works(string encoding)
        {
            var _params = Params.FromEncoding(encoding);
            var solveResult = _params.Solve(60);
            var proof = solveResult.Solution.ToProof();
            var b = Solution.FromProof(proof).VerifySolutionAndMethod(_params, 30);
        }
    }
}
