﻿using OpenQA.Selenium;
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
        private IWebElement OkNumpadButton => Driver.FindElement(By.XPath("//button[@id='keypad-key-ok']"), 30);   
        private IWebElement RemoveItem => Driver.GetAllElements(By.XPath("//div[contains(@class,'RemoveLineButton')]")).FirstOrDefault();
        private IWebElement ConfirmRemoveItem => Driver.FindElement(By.XPath("//div[contains(@id,'remove_article_line')]"));
        private IWebElement SearchItemBar => Driver.FindElement(By.XPath("//div[contains(@class, 'SearchBar__Form')]"));
        private IWebElement SearchItemInput => Driver.FindElement(By.XPath("//div[contains(@class, 'SearchBar__Form')]//input"));
        private IWebElement TotalButton => Driver.FindElement(By.XPath("//button[@id='payment_modal_button_subtotal']"), 30);
        private IWebElement ValidateItemAdded(string itemNumber) => Driver.FindElement(By.XPath($"//div[text() = '{itemNumber}']"), 30);
        private IWebElement TotalAmount => Driver.FindElement(By.XPath($"//div[contains(@class,'backgroundFormatter__Container')]//span"), 30);
        
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

        public void RemoveItemFromBasket()
        {
            Driver.WaitForElementToBeClickable(RemoveItem);
            RemoveItem.Click();
            Thread.Sleep(1000);
            ConfirmRemoveItem.Click();
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

        public double GetTotalAmount()
        {
            var amount = TotalAmount.Text;
            amount = amount.Replace("€", "");
            return double.Parse(amount);
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
