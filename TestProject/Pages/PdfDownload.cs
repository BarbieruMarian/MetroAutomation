using TestFramework.Configuration;
using TestFramework.Selenium.Interfaces;
using TestProject.Pages.Base;

namespace TestProject.Pages
{
    public class PdfDownload : BasePage
    {
        public PdfDownload(IDriverProxy driver) : base(driver)
        {
        }
        public void DownloadPDF(string invoiceID)
        {
            Driver.Navigate(Config.PtInvoiceDownloadPDF + $"{invoiceID}.pdf");
        }
    }
}
