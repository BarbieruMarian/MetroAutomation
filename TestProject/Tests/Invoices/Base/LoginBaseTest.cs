using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TestFramework.Configuration;
using TestFramework.Helper;
using TestFramework.Selenium.Interfaces;
using TestFramework.Test;
using TestProject.Pages;
using Xunit;

namespace TestProject.Tests.Invoices.Base
{
    public class LoginBaseTest : BaseTest
    {
        public LoginBaseTest(IDriverType browserDriverType) : base(browserDriverType)
        {
        }

        protected void Login(string supervisor, string customer)
        {

        }

        protected string AddItemToBasketAndGetTransactionId(string itemID)
        {
            var basketPage = new Basket(Driver);
            Assert.True(basketPage.IsAtBasket(), "The basket page could not be loaded");
            var transactionID = basketPage.GetTransactionID();

            basketPage.AddItem(itemID);
            Assert.True(basketPage.ValidateItemWasAdded(itemID), "The item was not added in basket");
            basketPage.PressTotal();
            return transactionID;
        }


        protected void PayWithCash()
        {
            var paymentPage = new Payment(Driver);
            var loginPage = new Pages.Login(Driver);
            Assert.True(paymentPage.IsAtPayment(), "The payment page could not be loaded");
            paymentPage.AddCashPayment();
            Assert.True(paymentPage.WasPaymentSuccessful(), "The invoice popup was not loaded");
            paymentPage.NextInvoice();
            Assert.True(loginPage.IsAtCustomerLogin(), "The customer login page could not be loaded");
        }

        protected string GetInvoiceID(string transactionID)
        {
            var apiHelper = new RestManager(Config.PTInvoiceEndpoint);
            var client2 = apiHelper.SetURL($"/invoices?TransactionId={transactionID}");
            var request2 = apiHelper.CreateGetRequest();
            var result = apiHelper.GetResponse(client2, request2);
            Assert.True(result.Content != string.Empty, $"The Invoice API GET Response for the transaction id: {transactionID} was empty.");
            var json = (JObject)JsonConvert.DeserializeObject(result.Content);
            var invoiceID = json.SelectToken("items[0].id").Value<string>();
            return invoiceID;
        }

        protected override void BeforeEach()
        {
            base.BeforeEach();
            FileManager.DeleteFilesFromFolder(Config.PDFDownloadsFolder);
        }

        protected override void AfterEach()
        {
            base.AfterEach();
        }
    }
}
