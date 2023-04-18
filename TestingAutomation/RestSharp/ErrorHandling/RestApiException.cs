using System.Net;

namespace TestFramework.RestSharp.ErrorHandling
{
    public class RestApiException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public RestApiError Error { get; set; }

        public RestApiException(HttpStatusCode statusCode, RestApiError error)
            : base($"API request failed with status code {statusCode}: {error?.Message}")
        {
            StatusCode = statusCode;
            Error = error;
        }
    }
}
