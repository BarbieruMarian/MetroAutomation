using OpenQA.Selenium;
using TestFramework.Extensions;
using TestFramework.Selenium.Interfaces;

namespace TestProject.Pages
{
    public class Payment
    {
        private readonly IDriverProxy driver;
        public Payment(IDriverProxy driver)
        {
            this.driver = driver;
        }

        #region Elements
        private IWebElement CashPaymentButton => driver.FindElement(By.XPath("//button[@id='btn-payment-method-1']"), 30);
        private IWebElement EndInvoiceButton => driver.FindElement(By.XPath("//button[@id='btn-Payment-EOP']"), 30);
        private IWebElement NextInvoiceButton => driver.FindElement(By.XPath("//button[@id='btn-next-invoice']"), 60);

        #endregion

        #region Actions
        public void AddCashPayment()
        {
            CashPaymentButton.Click();
            Thread.Sleep(1000);
            EndInvoiceButton.Click();
        }

        public void NextInvoice()
        {
            Thread.Sleep(1000);
            NextInvoiceButton.Click();
        }

        public bool IsAtPayment()
        {
            Thread.Sleep(2000);
            return CashPaymentButton.Displayed;
        }

        public bool WasPaymentSuccessful()
        {
            Thread.Sleep(2000);
            return NextInvoiceButton.Displayed;
        }
        #endregion
    }
}
