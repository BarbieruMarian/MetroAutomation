using Newtonsoft.Json;
using RestSharp;
using TestFramework.RestSharp.ErrorHandling;
using TestFramework.RestSharp.Interfaces;

namespace TestFramework.RestSharp
{
    public class RestApiClient : IApiClient
    {
        private readonly string _baseUrl;
        private readonly RestClient _client;

        public RestApiClient(string baseUrl)
        {
            _baseUrl = baseUrl;
            _client = new RestClient(_baseUrl);
        }

        public T Get<T>(string resource, Dictionary<string, object> parameters = null)
        {
            var request = new RestRequest(resource, Method.Get);

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    request.AddQueryParameter(parameter.Key, parameter.Value.ToString());
                }
            }

            var response = _client.Execute<T>(request);

            if (!response.IsSuccessful)
            {
                var error = JsonConvert.DeserializeObject<RestApiError>(response.Content);
                throw new RestApiException(response.StatusCode, error);
            }

            return response.Data;
        }

        public T Post<T>(string resource, object data)
        {
            var request = new RestRequest(resource, Method.Post);
            request.AddJsonBody(data);

            var response = _client.Execute<T>(request);

            if (!response.IsSuccessful)
            {
                var error = JsonConvert.DeserializeObject<RestApiError>(response.Content);
                throw new RestApiException(response.StatusCode, error);
            }

            return response.Data;
        }

        public T Put<T>(string resource, object data)
        {
            var request = new RestRequest(resource, Method.Put);
            request.AddJsonBody(data);

            var response = _client.Execute<T>(request);

            if (!response.IsSuccessful)
            {
                var error = JsonConvert.DeserializeObject<RestApiError>(response.Content);
                throw new RestApiException(response.StatusCode, error);
            }

            return response.Data;
        }

        public T Delete<T>(string resource)
        {
            var request = new RestRequest(resource, Method.Delete);

            var response = _client.Execute<T>(request);

            if (!response.IsSuccessful)
            {
                var error = JsonConvert.DeserializeObject<RestApiError>(response.Content);
                throw new RestApiException(response.StatusCode, error);
            }

            return response.Data;
        }
    }
}
