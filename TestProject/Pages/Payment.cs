using OpenQA.Selenium;
using TestFramework.Selenium.Interfaces;
using TestProject.Pages.Base;

namespace TestProject.Pages
{
    public class Payment : BasePage
    {
        public Payment(IDriverProxy driver) : base(driver)
        {
        }

        #region Elements
        private IWebElement CashPaymentButton => Driver.FindElement(By.XPath("//button[@id='btn-payment-method-1']"), 30);
        private IWebElement EndInvoiceButton => Driver.FindElement(By.XPath("//button[@id='btn-Payment-EOP']"), 30);
        private IWebElement NextInvoiceButton => Driver.FindElement(By.XPath("//button[@id='btn-next-invoice']"), 60);
        #endregion

        #region Actions
        public void AddCashPayment()
        {
            CashPaymentButton.Click();
            Thread.Sleep(2500);
            EndInvoiceButton.Click();
        }

        public void NextInvoice()
        {
            Thread.Sleep(2500);
            NextInvoiceButton.Click();
        }

        public bool IsAtPayment()
        {
            Thread.Sleep(2500);
            return CashPaymentButton.Displayed;
        }

        public bool WasPaymentSuccessful()
        {
            Thread.Sleep(2500);
            return NextInvoiceButton.Displayed;
        }
        #endregion
    }
}
