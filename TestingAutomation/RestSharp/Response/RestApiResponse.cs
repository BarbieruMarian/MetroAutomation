namespace TestFramework.RestSharp.Response
{
    public class RestApiResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string ErrorMessage { get; set; }

        public RestApiResponse(T data)
        {
            Success = true;
            Data = data;
        }

        public RestApiResponse(string errorMessage)
        {
            Success = false;
            ErrorMessage = errorMessage;
        }
    }
}
