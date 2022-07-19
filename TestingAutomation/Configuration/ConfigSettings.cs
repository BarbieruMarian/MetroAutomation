using Newtonsoft.Json;

namespace TestFramework.Configuration
{
    public class ConfigSettings
    {
        [JsonProperty("mPosAirPT")]
        public string? MPOSAirPT { get; set; }

        [JsonProperty("superviser")]
        public string? Superviser { get; set; }

        [JsonProperty("customer")]
        public string? Customer { get; set; }

        [JsonProperty("pTInvoiceEndpoint")]
        public string ? PTInvoiceEndpoint { get; set; }   
        
        [JsonProperty("ptInvoiceDownloadPDF")]
        public string ? PtInvoiceDownloadPDF { get; set; }

        [JsonProperty("BrowserType")]
        public string? BrowserType{ get; set; }
    }
}
