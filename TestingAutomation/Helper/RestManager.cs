using RestSharp;

namespace TestFramework.Helper
{
    public class RestManager
    {
        public RestClient restClient;
        public RestRequest restRequest;
        public string baseURL;

        public RestManager(string baseURL)
        {
            this.baseURL = baseURL;
        }

        public RestClient SetURL(string endpoint)
        {
            var url = baseURL + endpoint;
            restClient = new RestClient(url);
            return restClient;
        }

        public RestRequest CreatePostRequest(string payload)
        {
            restRequest = new RestRequest("", Method.Post);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.AddParameter("application/json", payload, ParameterType.RequestBody);
            return restRequest;
        }

        public RestRequest CreateGetRequest()
        {
            var restRequest = new RestRequest();
            return restRequest;
        }

        public RestResponse GetResponse(RestClient client, RestRequest restRequest)
        {
            return client.Execute(restRequest);
        }
    }
}
