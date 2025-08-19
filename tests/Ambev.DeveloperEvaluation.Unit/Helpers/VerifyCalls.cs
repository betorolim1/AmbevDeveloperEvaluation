using NSubstitute;

namespace Ambev.DeveloperEvaluation.Unit.Helpers
{
    public static class VerifyCalls
    {
        /// <summary>
        /// Verify if the substitute has no other calls than the expected ones.
        /// </summary>
        /// <param name="substitute"></param>
        /// <param name="expectedCalls"></param>
        /// <exception cref="Exception"></exception>
        public static void VerifyNoOtherCalls(this object substitute, int expectedCalls)
        {
            var calls = substitute.ReceivedCalls().ToList();

            if (calls.Count != expectedCalls)
            {
                throw new Exception(
                    $"Expected {expectedCalls} call(s), but was {calls.Count} call(s)\n" +
                    "Calls:\n" +
                    string.Join("\n", calls.Select(c => c.GetMethodInfo().Name))
                );
            }
        }
    }
}
