using OpenQA.Selenium;
using TestFramework.Configuration;
using TestFramework.Helper;
using TestFramework.Selenium.Interfaces;
using TestProject.Pages.Base;

namespace TestProject.Pages
{
    public class Login : BasePage
    {
        public Login(IDriverProxy driver) : base(driver)
        {
        }

        #region Elements
        private IWebElement KeyboardButton => Driver.FindElement(By.XPath("//button[@id='button-use-keypad']"), 30);
        private IWebElement EmployeeIDInput => Driver.FindElement(By.XPath("//input[@class='css-1etmqih']"), 30);
        private IWebElement CustomerIDInput => Driver.FindElement(By.XPath("//input[@id='customerBarcode']"), 30);
        private IWebElement OkLoginButton => Driver.FindElement(By.XPath("//button[@id='on-screen-keypad-key-ok']"), 30);
        private IWebElement PasswordBtn1 => Driver.FindElement(By.XPath("//button[@id='on-screen-keypad-key-1']"), 30);
        private IWebElement PasswordBtn2 => Driver.FindElement(By.XPath("//button[@id='on-screen-keypad-key-2']"), 30);
        private IWebElement PasswordBtn3 => Driver.FindElement(By.XPath("//button[@id='on-screen-keypad-key-3']"), 30);
        private IWebElement LoginAssertion => Driver.FindElement(By.XPath("//div[@class='m-media-visual']"), 30);
        private IWebElement CustomerAssertion => Driver.FindElement(By.XPath("//*[text()='Customer Identification']"), 30);       
        #endregion

        #region Actions
        public void GoTo()
        {
            Driver.Navigate(Config.MPOSAirPT);
            LocalStorage.Setup(Driver);
        }
        public void LoginWithSuperviser(string user)
        {
            Driver.WaitForElementToBeClickable(KeyboardButton);   
            KeyboardButton.Click();
            Thread.Sleep(750);
            EmployeeIDInput.SendKeys(user);
            Thread.Sleep(750);
            OkLoginButton.Click();

            Thread.Sleep(1250);
            PasswordBtn1.Click();
            Thread.Sleep(750);
            PasswordBtn2.Click();
            Thread.Sleep(750);
            PasswordBtn3.Click();
            OkLoginButton.Click();
        }

        public void LoginWithCustomer(string user)
        {
            Driver.WaitForElementToBeClickable(CustomerIDInput);
            CustomerIDInput.SendKeys(user);
            OkLoginButton.Click();
        }

        public bool IsAtLogin()
        {
            Driver.WaitForElementToBeDisplayed(LoginAssertion);
            return LoginAssertion.Displayed;
        } 

        public bool IsAtCustomerLogin()
        {
            Driver.WaitForElementToBeDisplayed(CustomerAssertion);
            return CustomerAssertion.Displayed;
        }
        #endregion
    }
}
