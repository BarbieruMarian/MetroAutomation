using AventStack.ExtentReports;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TestFramework.Configuration;
using TestFramework.Helper;
using TestFramework.Selenium.Interfaces;
using TestProject.Pages;
using TestProject.PDFPointOfTruth;
using TestProject.Tests.Invoices.Base;
using Xunit;

namespace TestProject.Tests
{
    public class NormalInvoice : InvoicesBaseTest
    {
        private const string ItemToAdd = "73471";
        public NormalInvoice(IDriverType browserDriverType) : base(browserDriverType)
        {
        }

        [UnitTestUtilities.Attributes.CountryFact("ES")]
        public void Normal_Invoice_Cash_Payment()
        {
            try
            {
                test = extent.CreateTest("Normal_Invoice_Cash_Payment").Info("Normal Invoice Test");

                test.Log(Status.Info, "Step 1 - Open App, Setting App Local Storage, Login with Superviser and Customer");
                Login(Config.Superviser, Config.Customer);

                test.Log(Status.Info, "Step 2 - Add an item to basket");
                var transactionID = AddItemToBasketAndGetTransactionId(ItemToAdd);

                test.Log(Status.Info, "Step 3 - Make the payment");
                PayWithCash();

                test.Log(Status.Info, "Step 4 - Get the invoiceID by calling GetInvoiceID API");
                var invoiceID = GetInvoiceID(transactionID);

                test.Log(Status.Info, "Step 5 - Get Invoice PDF on local - downloaded to Downloads Folder");
                //do it with a rest call to have it in a string
                var invoicePdfAPIPage = new PdfDownload(Driver);
                invoicePdfAPIPage.DownloadPDF(invoiceID);
                Thread.Sleep(5000);

                test.Log(Status.Info, "Step 6 - Compare expected PDF result with the downloaded PDF");
                var downloadedPDFPath = FileManager.GetMostRecentFileFromFolder(Config.PDFDownloadsFolder);
                FileManager.CreateTextFile(Config.PDFParsedToTextFolder, "todayResult.txt");
                PDFManager.ParsePDFToFile(downloadedPDFPath, Config.PDFParsedToTextFolder + "todayResult.txt");
                var textFromNotepad = FileManager.ReadFromNotepad(Config.PDFParsedToTextFolder + "todayResult.txt");

                var isHeaderOneContained = textFromNotepad.Contains(NormalInvoiceDataPOT.Header1);
                var isHeaderTwoContained = textFromNotepad.Contains(NormalInvoiceDataPOT.Header2);
                var isHeaderThreeContained = textFromNotepad.Contains(NormalInvoiceDataPOT.Header3);
                var isClientProductHeaderContained = textFromNotepad.Contains(NormalInvoiceDataPOT.ClientProductHeader);
                var isProductInfoContained = textFromNotepad.Contains(NormalInvoiceDataPOT.ProductInfo);
                var isFooterContained = textFromNotepad.Contains(NormalInvoiceDataPOT.Footer);
                Assert.True(isHeaderOneContained, "HeaderOne was different from the expected result.");
                Assert.True(isHeaderTwoContained, "HeaderTwo was different from the expected result.");
                Assert.True(isHeaderThreeContained, "HeaderThree was different from the expected result.");
                Assert.True(isClientProductHeaderContained, "ClientProductHeader was different from the expected result.");
                Assert.True(isProductInfoContained, "ProductInfo was different from the expected result.");
                Assert.True(isFooterContained, "Footer was different from the expected result.");
                test.Pass("Test Passed");
            }
            catch (Exception e)
            {
                test.Fail($"Test failed: {e.Message}");
                Assert.True(e.Message == String.Empty, $"Test Failed: {e.Message}");
                throw;
            }
        }
    }
}
