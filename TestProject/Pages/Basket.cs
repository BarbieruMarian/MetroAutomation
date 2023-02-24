using OpenQA.Selenium;
using TestFramework.Selenium.Interfaces;
using TestProject.Pages.Base;

namespace TestProject.Pages
{
    public class Basket : BasePage
    {
        public Basket(IDriverProxy driver) : base(driver)
        {
        }

        #region Elements
        private IWebElement EnterItemTextbox => Driver.FindElement(By.XPath("//input[@id='input-checkout-article-id']"), 30);
        private IWebElement OkNumpadButton => Driver.FindElement(By.XPath("//button[@id='on-screen-keypad-key-ok']"), 30);
        private IWebElement TotalButton => Driver.FindElement(By.XPath("//button[@id='payment_modal_button_subtotal']"), 30);
        private IWebElement ValidateItemAdded(string itemNumber) => Driver.FindElement(By.XPath($"//div//span[text()='{itemNumber}']"), 30);
        #endregion

        #region Actions
        public void AddItem(string itemName)
        {
            Driver.WaitForElementToBeClickable(EnterItemTextbox);
            EnterItemTextbox.SendKeys(itemName);
            Thread.Sleep(1000);
            OkNumpadButton.Click();
        }

        public void PressTotal()
        {
            Thread.Sleep(1750);
            TotalButton.Click();
        }

        public bool IsAtBasket()
        {
            Thread.Sleep(3000);
            Driver.WaitForElementToBeClickable(TotalButton);
            return TotalButton.Displayed;
        }

        public bool ValidateItemWasAdded(string itemNumber)
        {
            Driver.WaitForElementToBeClickable(ValidateItemAdded(itemNumber));
            return ValidateItemAdded(itemNumber).Displayed;
        }

        public string GetTransactionID()
        {
            var url = Driver.Url;
            var transaction = "transactions/";
            int startIndex = url.IndexOf(transaction);
            var transactionID = url.Substring(startIndex + transaction.Length);

            return transactionID;
        }
        #endregion
    }
}
