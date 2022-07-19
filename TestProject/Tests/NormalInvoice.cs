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

        [Fact]
        public void Normal_Invoice_Cash_Payment()
        {        
            //step 3 -  Open App, Setting App Local Storage, Login with Superviser and Customer
            var loginPage = new Login(Driver);
            loginPage.GoTo();
            Assert.True(loginPage.IsAtLogin(), "The main page could not be loaded");
            loginPage.LoginWithSuperviser(Config.Superviser);

            Assert.True(loginPage.IsAtCustomerLogin(), "The customer login page could not be loaded");
            loginPage.LoginWithCustomer(Config.Customer);

            //step 4 - Add an item to basket

            var basketPage = new Basket(Driver);
            Assert.True(basketPage.IsAtBasket(), "The basket page could not be loaded");
            var transactionID = basketPage.GetTransactionID();

            basketPage.AddItem(ItemToAdd);
            Assert.True(basketPage.ValidateItemWasAdded(ItemToAdd), "The item was not added in basket");

            //step 5 - Make the payment
            basketPage.PressTotal();
            var paymentPage = new Payment(Driver);
            Assert.True(paymentPage.IsAtPayment(), "The payment page could not be loaded");
            paymentPage.AddCashPayment();
            Assert.True(paymentPage.WasPaymentSuccessful(), "The invoice popup was not loaded");

            paymentPage.NextInvoice();
            Assert.True(loginPage.IsAtCustomerLogin(), "The customer login page could not be loaded");

            //Step 6 - Get the invoiceID by calling GetInvoiceID API
            //var transactionID = "d159f87f-7f93-49e5-9941-ade5ed5a3c29";
            var apiHelper = new RestManager(Config.PTInvoiceEndpoint);
            var client2 = apiHelper.SetURL($"/invoices?TransactionId={transactionID}");
            var request2 = apiHelper.CreateGetRequest();
            var result = apiHelper.GetResponse(client2, request2);
            Assert.True(result.Content != string.Empty, $"The Invoice API GET Response for the transaction id: {transactionID} was empty.");

            var json = (JObject)JsonConvert.DeserializeObject(result.Content);
            var invoiceID = json.SelectToken("items[0].id").Value<string>();

            // Step 7 - Get Invoice PDF on local - downloaded to "Downloads" Folder
            var invoicePdfAPIPage = new PdfDownload(Driver);
            invoicePdfAPIPage.DownloadPDF(invoiceID);
            Thread.Sleep(5000);

            // Step 8 - Compare expected PDF result with the downloaded PDF   
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
        }
    }
}
