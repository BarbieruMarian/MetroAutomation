using Microsoft.Extensions.Configuration;
using TestFramework.Selenium.WebDriver;
using Winton.Extensions.Configuration.Consul;

namespace TestFramework.Configuration
{
    public class ConfigReader
    {
        public static void InitializeSettings(string jsonSection)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appconfig.json", optional: false, reloadOnChange: true);

            var environment = GetEnvironmentValue(builder);

            builder.AddJsonFile($"appconfig.{environment}.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configurationRoot = builder.Build();

            Config.MPOSAirPT = configurationRoot.GetSection(jsonSection).Get<ConfigSettings>().MPOSAirPT;
            Config.Superviser = configurationRoot.GetSection(jsonSection).Get<ConfigSettings>().Superviser;
            Config.Customer = configurationRoot.GetSection(jsonSection).Get<ConfigSettings>().Customer;
            Config.PTInvoiceEndpoint = configurationRoot.GetSection(jsonSection).Get<ConfigSettings>().PTInvoiceEndpoint;
            Config.PtInvoiceDownloadPDF = configurationRoot.GetSection(jsonSection).Get<ConfigSettings>().PtInvoiceDownloadPDF;
            Config.PDFDownloadsFolder = configurationRoot.GetSection(jsonSection).Get<ConfigSettings>().PDFDownloadsFolder;
            Config.PDFParsedToTextFolder = configurationRoot.GetSection(jsonSection).Get<ConfigSettings>().PDFParsedToTextFolder;
            Config.PageLoadImplicitWait = configurationRoot.GetSection(jsonSection).Get<ConfigSettings>().PageLoadImplicitWait;
            Config.ImplicitWait = configurationRoot.GetSection(jsonSection).Get<ConfigSettings>().ImplicitWait; ;
            Config.ConsulConfiguration = BuildConsul(configurationRoot, builder);

            var browserType = configurationRoot.GetSection(jsonSection).Get<ConfigSettings>().BrowserType;
            switch (browserType)
            {
                case "Chrome":
                    Config.BrowserType = BrowserType.Chrome;
                    break;

                case "Firefox":
                    Config.BrowserType = BrowserType.Firefox;
                    break;

                default:
                    Config.BrowserType = BrowserType.Chrome;
                    break;
            }
        }
        private static IConfiguration BuildConsul(IConfigurationRoot configurationRoot, IConfigurationBuilder builder)
        {
            var consulRegistrationOptions = configurationRoot
                .GetSection("ConsulOptions")
                .Get<ConsulOptions>();

            var consulOptions = consulRegistrationOptions.GetDefaultConsulOptions();
            if (consulOptions is null)
            {
                return null;
            }

            if (consulRegistrationOptions.IsHqEnvironment)
            {
                var hqConsulKey = consulRegistrationOptions.GetHqConsulKey();
                builder.AddConsul(hqConsulKey, consulOptions);
                return builder.Build();
            }

            var defaultPathConsulKey = consulRegistrationOptions.GetDefaultConsulKey();
            var storePathConsulKey = consulRegistrationOptions.GetStoreConsulKey();

            builder.AddConsul(defaultPathConsulKey, consulOptions);
            builder.AddConsul(storePathConsulKey, consulOptions);
            return builder.Build();

        }
        private static string GetEnvironmentValue(IConfigurationBuilder builder)
        {
            var result = Environment.GetEnvironmentVariable("MPOSAIR_ENVIRONMENT");
            var countryCode = Environment.GetEnvironmentVariable("COUNTRY_CODE");

            if (string.IsNullOrWhiteSpace(result))
            {
                var config = builder.Build();
                result = config.GetSection("DefaultEnvironmentForTests").Value;
            }
            else
            {
                result = $"{result}-{countryCode}";
            }

            Console.WriteLine($"Environment is: {result}");
            return result;
        }
    }
}
