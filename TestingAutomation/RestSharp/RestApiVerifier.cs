using TestFramework.RestSharp.Response;

namespace TestFramework.RestSharp
{
    public static class RestApiVerifier
    {
        public static void VerifyResponse<T>(RestApiResponse<T> response, Action<T> verifier)
        {
            if (!response.Success)
            {
                throw new Exception(response.ErrorMessage);
            }

            verifier(response.Data);
        }

        public static void VerifyResponse<T>(RestApiResponse<T> response, Func<T, bool> predicate, string errorMessage)
        {
            if (!response.Success)
            {
                throw new Exception(response.ErrorMessage);
            }

            if (!predicate(response.Data))
            {
                throw new Exception(errorMessage);
            }
        }
    }
}
