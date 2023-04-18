namespace TestFramework.RestSharp.Interfaces
{
    public interface IApiClient
    {
        T Get<T>(string resource, Dictionary<string, object> parameters = null);
        T Post<T>(string resource, object data);
        T Put<T>(string resource, object data);
        T Delete<T>(string resource);
    }
}
