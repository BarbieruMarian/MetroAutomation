using OpenQA.Selenium;
using TestFramework.Configuration;
using TestFramework.Selenium.Interfaces;

namespace TestProject.Pages
{
    public class InvoicePdfAPI
    {
        private readonly IDriverProxy driver;
        public InvoicePdfAPI(IDriverProxy driver)
        {
            this.driver = driver;
        }

        public void DownloadPDF(string invoiceID)
        {
            driver.Navigate(Config.PtInvoiceDownloadPDF + $"{invoiceID}.pdf");
        }
    }
}
