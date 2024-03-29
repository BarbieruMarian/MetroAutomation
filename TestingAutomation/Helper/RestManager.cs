﻿using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net;

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

        public RestRequest CreatePutRequest(string jsonString)
        {
            restRequest = new RestRequest("", Method.Put);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.AddParameter("application/json", jsonString, ParameterType.RequestBody);
            return restRequest;
        }

        public RestRequest CreatePatchRequest(string jsonString)
        {
            restRequest = new RestRequest("", Method.Patch);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.AddParameter("application/json", jsonString, ParameterType.RequestBody);
            return restRequest;
        }

        public RestRequest CreateDeleteRequest()
        {
            restRequest = new RestRequest("", Method.Delete);
            restRequest.AddHeader("Accept", "application/json");
            return restRequest;
        }

        public RestResponse GetResponse(RestClient client, RestRequest restRequest)
        {
            return client.Execute(restRequest);
        }

        public bool IsSuccessStatusCode(HttpStatusCode responseCode)
        {
            var numericResponse = (int)responseCode;

            const int statusCodeOk = (int)HttpStatusCode.OK;

            const int statusCodeBadRequest = (int)HttpStatusCode.BadRequest;

            return numericResponse >= statusCodeOk &&
                   numericResponse < statusCodeBadRequest;
        }

        public string DeserializeResponseToStringUsingJObject(RestResponse restResponse, string responseObj)
        {
            var jObject = JObject.Parse(restResponse.Content);
            return jObject[responseObj]?.ToString();
        }
    }
}
