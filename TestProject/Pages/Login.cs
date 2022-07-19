using OpenQA.Selenium;
using TestFramework.Configuration;
using TestFramework.Extensions;
using TestFramework.Helper;
using TestFramework.Selenium.Interfaces;

namespace TestProject.Pages
{
    public class Login
    {
        private readonly IDriverProxy driver;
        public Login(IDriverProxy driver)
        {
            this.driver = driver;
        }

        #region Elements
        private IWebElement KeyboardButton => driver.FindElement(By.XPath("//button[@id='button-use-keypad']"), 30);
        private IWebElement EmployeeIDInput => driver.FindElement(By.XPath("//input[@class='css-1etmqih']"), 30);
        private IWebElement CustomerIDInput => driver.FindElement(By.XPath("//input[@id='customerBarcode']"), 30);
        private IWebElement OkLoginButton => driver.FindElement(By.XPath("//button[@id='on-screen-keypad-key-ok']"), 30);
        private IWebElement PasswordBtn1 => driver.FindElement(By.XPath("//button[@id='on-screen-keypad-key-1']"), 30);
        private IWebElement PasswordBtn2 => driver.FindElement(By.XPath("//button[@id='on-screen-keypad-key-2']"), 30);
        private IWebElement PasswordBtn3 => driver.FindElement(By.XPath("//button[@id='on-screen-keypad-key-3']"), 30);
        private IWebElement LoginAssertion => driver.FindElement(By.XPath("//div[@class='m-media-visual']"), 30);
        private IWebElement CustomerAssertion => driver.FindElement(By.XPath("//*[text()='Customer Identification']"), 30);

        
        #endregion

        #region Actions

        public void GoTo()
        {
            driver.Navigate(Config.MPOSAirPT);
            SettingLocalStorage.Setup(driver);
        }
        public void LoginWithSuperviser(string user)
        {
            driver.WaitForElementToBeClickable(KeyboardButton);   
            KeyboardButton.Click();
            Thread.Sleep(500);
            EmployeeIDInput.SendKeys(user);
            Thread.Sleep(500);
            OkLoginButton.Click();

            Thread.Sleep(1000);
            PasswordBtn1.Click();
            Thread.Sleep(500);
            PasswordBtn2.Click();
            Thread.Sleep(500);
            PasswordBtn3.Click();
            OkLoginButton.Click();
        }

        public void LoginWithCustomer(string user)
        {
            driver.WaitForElementToBeClickable(CustomerIDInput);
            CustomerIDInput.SendKeys(user);
            OkLoginButton.Click();
        }

        public bool IsAtLogin()
        {
            driver.WaitForElementToBeDisplayed(LoginAssertion);
            return LoginAssertion.Displayed;
        } 

        public bool IsAtCustomerLogin()
        {
            driver.WaitForElementToBeDisplayed(CustomerAssertion);
            return CustomerAssertion.Displayed;
        }
        #endregion
    }
}
