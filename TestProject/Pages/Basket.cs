using OpenQA.Selenium;
using TestFramework.Extensions;
using TestFramework.Selenium.Interfaces;

namespace TestProject.Pages
{
    public class Basket
    {
        private readonly IDriverProxy driver;
        public Basket(IDriverProxy driver)
        {
            this.driver = driver;
        }

        #region Elements
        private IWebElement EnterItemTextbox => driver.FindElement(By.XPath("//input[@id='input-checkout-article-id']"), 30);
        private IWebElement OkNumpadButton => driver.FindElement(By.XPath("//button[@id='on-screen-keypad-key-ok']"), 30);
        private IWebElement TotalButton => driver.FindElement(By.XPath("//button[@id='btn-checkout-total']"), 30);
        private IWebElement ValidateItemAdded(string itemNumber) => driver.FindElement(By.XPath($"//div//span[text()='{itemNumber}']"), 30);

        #endregion

        #region Actions
        public void AddItem(string itemName)
        {
            driver.WaitForElementToBeClickable(EnterItemTextbox);
            EnterItemTextbox.SendKeys(itemName);
            Thread.Sleep(500);
            OkNumpadButton.Click();
        }

        public void PressTotal()
        {
            Thread.Sleep(1500);
            TotalButton.Click();
        }

        public bool IsAtBasket()
        {
            driver.WaitForElementToBeClickable(TotalButton);
            return TotalButton.Displayed;
        }

        public bool ValidateItemWasAdded(string itemNumber)
        {
            driver.WaitForElementToBeClickable(ValidateItemAdded(itemNumber));
            return ValidateItemAdded(itemNumber).Displayed;
        }

        public string GetTransactionID()
        {
            var url = driver.Url;
            var transaction = "transactions/";
            int startIndex = url.IndexOf(transaction);
            var transactionID = url.Substring(startIndex + transaction.Length);

            return transactionID;
        }
        #endregion
    }
}
