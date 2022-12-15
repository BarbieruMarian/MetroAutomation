using TestFramework.Selenium.WebDriver;
using Microsoft.Extensions.Configuration;

namespace TestFramework.Configuration
{
    public class Config
    {
        #region TestOptions
        public static double PageLoadImplicitWait { get; set; }
        public static double ImplicitWait { get; set; }
        public static BrowserType BrowserType { get; set; }
        #endregion

        #region EnvironmentOptions
        public static int? StoreNumber { get; set; }
        public static string? PDFParsedToTextFolder { get; set; }
        public static string? PDFDownloadsFolder { get; set; }
        public static string? MPOSAirUI { get; set; }
        public static string? PTInvoiceEndpoint { get; set; }
        public static string? PtInvoiceDownloadPDF { get; set; }
        #endregion

        public static IConfiguration ConsulOptions { get; set; }

        #region TestData
        public static string? Superviser { get; set; }        
        public static string? SuperviserPIN { get; set; }
        public static string? Cashier { get; set; }
        public static string? CashierPIN { get; set; }
        public static string? Customer { get; set; }
        public static string? FiscalId { get; set; }
        public static int? TradeId { get; set; }
        #endregion
    }
}
