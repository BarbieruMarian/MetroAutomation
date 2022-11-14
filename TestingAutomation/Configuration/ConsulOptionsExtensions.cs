using System.Text;
using Winton.Extensions.Configuration.Consul;

namespace TestFramework.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public static class ConsulOptionsExtensions
    {
        /// <summary>
        /// Method for generating the store instance consul key
        /// </summary>
        /// <param name="options">Consul options from the configuration</param>
        /// <returns>Consul key</returns>
        public static string GetStoreConsulKey(this ConsulOptions options)
        {
            return CreateConsulPath(new[]
            {
                options.ServiceName,
                options.DeploymentType,
                options.DeploymentId.ToString()
            });
        }

        /// <summary>
        /// Method for generating the default instance consul key
        /// </summary>
        /// <param name="options">Consul options from the configuration</param>
        /// <returns>Consul key</returns>
        public static string GetDefaultConsulKey(this ConsulOptions options)
        {
            return CreateConsulPath(new[]
            {
                options.ServiceName,
                "default"
            });
        }

        /// <summary>
        /// Method for generating the HQ instance consul key
        /// </summary>
        /// <param name="options">Consul options from the configuration</param>
        /// <returns>Consul key</returns>
        public static string GetHqConsulKey(this ConsulOptions options)
        {
            return CreateConsulPath(new[]
            {
                options.ServiceName,
                "hq"
            });
        }

        /// <summary>
        /// Return consul configuration options based on consul options.
        /// </summary>
        /// <param name="consulRegistrationOptions">Consul options</param>
        /// <returns>Consul configuration options</returns>
        public static Action<IConsulConfigurationSource> GetDefaultConsulOptions(
            this ConsulOptions consulRegistrationOptions)
        {
            if (!consulRegistrationOptions.InUse ?? true)
            {
                return null;
            }

            if (!consulRegistrationOptions.ValidConfiguration())
            {
                return null;
            }

            return options =>
            {
                options.ReloadOnChange = true;
                options.Optional = true;
                options.ConsulConfigurationOptions = consulOptions =>
                {
                    consulOptions.Address = new Uri(consulRegistrationOptions.Instance);
                    consulOptions.Token = Encoding.UTF8.GetString(Convert.FromBase64String(consulRegistrationOptions.Token));
                };
                options.OnWatchException = _ => TimeSpan.FromSeconds(1);
                options.OnLoadException = exceptionContext => {exceptionContext.Ignore = true;};
            };
        }

        /// <summary>
        /// Method for checking if a set of consul options is valid for getting consul configurations
        /// </summary>
        /// <param name="consulRegistrationOptions"></param>
        /// <returns></returns>
        private static bool ValidConfiguration(this ConsulOptions consulRegistrationOptions)
        {
            var correctEnvironmentSetting = consulRegistrationOptions.DeploymentId.HasValue ||
                                            consulRegistrationOptions.IsHqEnvironment;

            var validConfiguration = correctEnvironmentSetting &&
                                     consulRegistrationOptions.ServiceName != null &&
                                     consulRegistrationOptions.DeploymentType != null;
            return validConfiguration;
        }

        private static string CreateConsulPath(string[] args)
            => string.Join('/', args);
    }
}
