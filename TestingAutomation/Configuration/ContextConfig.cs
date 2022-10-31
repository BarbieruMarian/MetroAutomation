using Microsoft.Extensions.Configuration;

namespace TestFramework.Configuration
{
    public class ContextConfig
    {
        private readonly IConfiguration _config;

        public ContextConfig()
        {
            _config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public int DefaultTimeout => int.Parse(_config["defaultTimeout"]);
        public string Mode => _config["mode"];
        public string Get(string key) => _config[key];
    }
}
