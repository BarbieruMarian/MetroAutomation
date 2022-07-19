using Microsoft.Extensions.Configuration;
using TestFramework.Selenium.WebDriver;

namespace TestFramework.Configuration
{
    public class ConfigReader
    {
        public static void InitializeSettings(string jsonSection)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appconfig.json");

            IConfigurationRoot configurationRoot = builder.Build();

            Config.MPOSAirPT = configurationRoot.GetSection(jsonSection).Get<ConfigSettings>().MPOSAirPT;
            Config.Superviser = configurationRoot.GetSection(jsonSection).Get<ConfigSettings>().Superviser;
            Config.Customer = configurationRoot.GetSection(jsonSection).Get<ConfigSettings>().Customer;
            Config.PTInvoiceEndpoint = configurationRoot.GetSection(jsonSection).Get<ConfigSettings>().PTInvoiceEndpoint;
            Config.PtInvoiceDownloadPDF = configurationRoot.GetSection(jsonSection).Get<ConfigSettings>().PtInvoiceDownloadPDF;
            Config.PDFDownloadsFolder = configurationRoot.GetSection(jsonSection).Get<ConfigSettings>().PDFDownloadsFolder;
            Config.PDFParsedToTextFolder = configurationRoot.GetSection(jsonSection).Get<ConfigSettings>().PDFParsedToTextFolder;

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
    }
}
