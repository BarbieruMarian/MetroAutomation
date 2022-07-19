using TestingAutomation.Driver;

namespace TestFramework.Configuration
{
    public class Config 
    {
        public static string? MPOSAirPT { get; set; }
        public static string? Superviser { get; set; }
        public static string? Customer { get; set; }
        public static string? PTInvoiceEndpoint { get; set; }
        public static string? PtInvoiceDownloadPDF { get; set; }
        public static BrowserType BrowserType { get; set; }
    }
}
