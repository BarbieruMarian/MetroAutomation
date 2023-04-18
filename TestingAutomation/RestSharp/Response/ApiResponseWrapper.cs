namespace TestFramework.RestSharp.Response
{
    public static class ApiResponseWrapper
    {
        public static RestApiResponse<T> CreateSuccessResponse<T>(T data)
        {
            return new RestApiResponse<T>(data);
        }

        public static RestApiResponse<T> CreateErrorResponse<T>(string errorMessage)
        {
            return new RestApiResponse<T>(errorMessage);
        }
    }
}
