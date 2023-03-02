using OpenQA.Selenium;
using TestFramework.Helper;
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
        private IWebElement EnterItem => Driver.FindElement(By.XPath("//div[contains(@class,'BasketInputKeypad__Input')]//input"), 30);
        private IWebElement Key2 => Driver.FindElement(By.XPath("//button[@id='keypad-key-2']"), 30);
        private IWebElement Key7 => Driver.FindElement(By.XPath("//button[@id='keypad-key-7']"), 30);
        private IWebElement KeyMultiply => Driver.FindElement(By.XPath("//button[@id='keypad-key-asterisk']"), 30);
        private IWebElement OkNumpadButton => Driver.FindElement(By.XPath("//button[@id='keypad-key-ok']"), 30);   
        private IWebElement RemoveItem => Driver.GetAllElements(By.XPath("//div[contains(@class,'RemoveLineButton')]")).FirstOrDefault();
        private IWebElement ConfirmRemoveItem => Driver.FindElement(By.XPath("//div[contains(@id,'remove_article_line')]"));
        private IWebElement SearchItemBar => Driver.FindElement(By.XPath("//div[contains(@class, 'SearchBar__Form')]"));
        private IWebElement SearchItemInput => Driver.FindElement(By.XPath("//div[contains(@class, 'SearchBar__Form')]//input"));
        private IWebElement TotalButton => Driver.FindElement(By.XPath("//button[@id='payment_modal_button_subtotal']"), 30);
        private IWebElement ValidateItemAdded(string itemNumber) => Driver.FindElement(By.XPath($"//div[text() = '{itemNumber}']"), 30);
        private IList<IWebElement> ValidateItemsAdded(string itemNumber) => Driver.GetAllElements(By.XPath($"//div[text() = '{itemNumber}']"), false, 30);
        private IWebElement NumberOfArticles => Driver.FindElement(By.XPath($"//div[contains(@class, 'counterFormatter__LineValue')]"), 30);      
        private IWebElement TotalAmount => Driver.FindElement(By.XPath($"//div[contains(@class,'backgroundFormatter__Container')]//span"), 30);
        private IWebElement BasketOptionsButton => Driver.FindElement(By.XPath($"//button[@id='button-0']"), 30);
        private IWebElement DiscountButton => Driver.FindElement(By.XPath($"//button[@id='add-article']"), 30);
        private IWebElement ToggleDiscountInput => Driver.FindElement(By.XPath($"//label[contains(@class, 'modeSwitch')]"), 30);
        private IWebElement DiscountInput => Driver.FindElement(By.XPath($"//input[@name='input-keypad-basket-coupons']"), 30);
        private IWebElement? OkNumpadButtonForDiscount => Driver.GetAllElements(By.XPath("//button[@id='keypad-key-ok']")).FirstOrDefault();


        private IWebElement InvoiceButton => Driver.FindElement(By.XPath($"//button[@id='button-1']"), 30);

        #endregion

        #region Actions
        public void AddItem(string itemName)
        {
            Driver.WaitForElementToBeClickable(EnterItem);
            EnterItem.SendKeys(itemName);
            Thread.Sleep(1000);
            OkNumpadButton.Click();
            Thread.Sleep(1000);
        }

        public void RepeatAddArticle(int numberOfTimes) 
        { 
            while(numberOfTimes > 0)
            {
                OkNumpadButton.Click();
                numberOfTimes--;
                Thread.Sleep(1000);
            }
        }

        public void RemoveItemFromBasket()
        {
            Driver.WaitForElementToBeClickable(RemoveItem);
            RemoveItem.Click();
            Thread.Sleep(1000);
            ConfirmRemoveItem.Click();
            Thread.Sleep(1000);
        }

        public void SearchItem(string item)
        {
            Driver.WaitForElementToBeClickable(SearchItemBar);
            SearchItemBar.Click();
            Thread.Sleep(500);
            Driver.SetElementFromDisabledToEnabled(SearchItemInput);
            SearchItemInput.SendKeys(item);
            Thread.Sleep(500);
        }

        public int GetNumberOfArticles()
        {
            return Utils.ExtractNumber(NumberOfArticles.Text);
        }

        public void InsertNumberOfArticles(int numberOfArticles, string articleToInsert)
        {
            EnterItem.SendKeys(numberOfArticles.ToString());
            Thread.Sleep(1000);
            KeyMultiply.Click();
            Thread.Sleep(1000);
            EnterItem.SendKeys(articleToInsert);
            Thread.Sleep(1000);
            OkNumpadButton.Click();
            Thread.Sleep(1000);
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

        public bool ValidateItemsWereAdded(string itemNumber, int expectedOccurences)
        {
            var occurencesFound = ValidateItemsAdded(itemNumber).Count;
            if (occurencesFound == expectedOccurences)
            {
                return true;
            }
            return false;
        }

        public double GetTotalAmount()
        {
            Thread.Sleep(1000);
            var amount = TotalAmount.Text;
            amount = amount.Replace("€", "");
            return double.Parse(amount);
        }

        public bool IsBasketButtonPresent()
        {
            var isPresent = BasketOptionsButton.Displayed;
            BasketOptionsButton.Click();
            return isPresent;
        }

        public bool IsInvoiceButtonPresent()
        {
            var isPresent = InvoiceButton.Displayed;
            InvoiceButton.Click();
            return isPresent;
        }

        public string GetTransactionID()
        {
            var url = Driver.Url;
            var transaction = "transactions/";
            int startIndex = url.IndexOf(transaction);
            var transactionID = url.Substring(startIndex + transaction.Length);

            return transactionID;
        }

        public void AddDiscountCoupon(string couponID)
        {
            BasketOptionsButton.Click();
            Thread.Sleep(1000);
            DiscountButton.Click();
            Thread.Sleep(1000);
            ToggleDiscountInput.Click();
            Thread.Sleep(1000);
            DiscountInput.SendKeys(couponID);
            Thread.Sleep(1000);
            OkNumpadButtonForDiscount.Click();
            Thread.Sleep(1000);
        }
        #endregion
    }
}
