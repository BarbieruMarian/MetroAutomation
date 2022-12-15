using Microsoft.Extensions.Configuration;
using TestFramework.Selenium.WebDriver;
using Winton.Extensions.Configuration.Consul;

namespace TestFramework.Configuration
{
    public class ConfigReader
    {
        public static void InitializeSettings()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appconfig.json", optional: false, reloadOnChange: true);

            var environment = GetEnvironmentValue(builder);

            builder.AddJsonFile($"appconfig.{environment}.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configurationRoot = builder.Build();

            // setting TestOptions
            Config.PageLoadImplicitWait = double.Parse(configurationRoot.GetSection("TestOptions:PageLoadImplicitWait").Value);
            Config.ImplicitWait = double.Parse(configurationRoot.GetSection("TestOptions:ImplicitWait").Value);
            var browserType = configurationRoot.GetSection("TestOptions:BrowserType").Value;
            SetBrowserType(browserType);

            // setting EnvironmentOptions
            Config.StoreNumber = int.Parse(configurationRoot.GetSection("EnvironmentOptions:StoreNumber").Value);
            Config.PDFParsedToTextFolder = configurationRoot.GetSection("EnvironmentOptions:PDFParsedToTextFolder").Value;
            Config.PDFDownloadsFolder = configurationRoot.GetSection("EnvironmentOptions:PDFDownloadsFolder").Value;
            Config.MPOSAirUI = configurationRoot.GetSection("EnvironmentOptions:MPOSAirUI").Value;
            Config.PTInvoiceEndpoint = configurationRoot.GetSection("EnvironmentOptions:PTInvoiceEndpoint").Value;
            Config.PtInvoiceDownloadPDF = configurationRoot.GetSection("EnvironmentOptions:PtInvoiceDownloadPDF").Value;

            // setting ConsulOptions
            Config.ConsulOptions = BuildConsul(configurationRoot, builder);

            // setting TestData
            Config.Superviser = configurationRoot.GetSection("TestData:Superviser").Value;
            Config.SuperviserPIN = configurationRoot.GetSection("TestData:SuperviserPIN").Value;
            Config.Cashier = configurationRoot.GetSection("TestData:Cashier").Value;
            Config.CashierPIN = configurationRoot.GetSection("TestData:CashierPIN").Value;
            Config.Customer = configurationRoot.GetSection("TestData:Customer").Value;
            Config.FiscalId = configurationRoot.GetSection("TestData:FiscalId").Value;
            Config.TradeId = int.Parse(configurationRoot.GetSection("TestData:TradeId").Value);
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

        private static void SetBrowserType(string browserType)
        {
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
    }
}
