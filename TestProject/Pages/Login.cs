using OpenQA.Selenium;
using System.Reflection;
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
        private IWebElement StartShiftButton => Driver.FindElement(By.XPath("//button[@id='btn-login-signIn']"));
        private IWebElement ButtonLoginLayout => Driver.FindElement(By.XPath("//button[@id='btn-login-layout']"), 30);
        private IWebElement ClientButtonLoginLayout => Driver.GetAllElements(By.XPath("//div[contains(@class, 'ModeSwitch')]")).FirstOrDefault();
        private IWebElement Body => Driver.FindElement(By.XPath(".//body"), 30);
        private IWebElement EmployeeIDInput => Driver.FindElement(By.XPath("//div[contains(@class, 'InputContainer')]//input"), 30);
        private IWebElement CustomerIDInput => Driver.FindElement(By.XPath("//input[@id='customerBarcode']"), 30);
        private IWebElement OkLoginButton => Driver.FindElement(By.XPath("//button[@id='keypad-key-ok']"), 30);
        private IWebElement PasswordBtn1 => Driver.FindElement(By.XPath("//button[@id='on-screen-keypad-key-1']"), 30);
        private IWebElement PasswordBtn2 => Driver.FindElement(By.XPath("//button[@id='on-screen-keypad-key-2']"), 30);
        private IWebElement InvalidUserBarcodeMessagePopup => Driver.FindElement(By.XPath("//div[contains(text(), 'invalid_barcode')]"), 30);
        private IWebElement InvalidUserCredentialsPopup => Driver.FindElement(By.XPath("//div[contains(text(), 'account_signin_invalid_user_credentials')]"), 30);
        private IWebElement CustomerAssertion => Driver.FindElement(By.XPath("//*[text()='Identificación Cliente ']"), 30);

        #endregion

        #region Actions
        public void GoTo()
        {
            Driver.Navigate("http://mpos-ui.buk30mast005000.mpos.madm.net/ ");
            LocalStorage.Setup(Driver);
        }
        public (bool success, string message) LoginWithSuperviser(string supervisorID, string password)
        {
            try
            {
                Driver.WaitForElementToBeClickable(StartShiftButton);
                StartShiftButton.Click();
                Driver.WaitForElementToBeClickable(ButtonLoginLayout);
                Driver.MoveToElement(ButtonLoginLayout);

                ButtonLoginLayout.Click();
                Thread.Sleep(750);
                EmployeeIDInput.SendKeys(supervisorID);
                Thread.Sleep(750);
                OkLoginButton.Click();
                Thread.Sleep(1000);
                EmployeeIDInput.SendKeys(password);
                Thread.Sleep(750);
                OkLoginButton.Click();
                return (true, "Login successful");
            }
            catch (Exception ex)
            {
                return (false, $"Login failed. Error message: {ex.Message}");
            }
        }

        public (bool success, string message) LoginWithCustomer(string customerID)
        {
            try
            {
                Driver.WaitForElementToBeClickable(ClientButtonLoginLayout);
                Driver.MoveToElement(ClientButtonLoginLayout);

                ClientButtonLoginLayout.Click();
                Thread.Sleep(750);
                EmployeeIDInput.SendKeys(customerID);
                Thread.Sleep(750);
                OkLoginButton.Click();
                return (true, "Login successful");
            }
            catch (Exception ex)
            {
                return (false, $"Login failed. Error message: {ex.Message}");
            }
        }

        public bool InvalidUserCredentials()
        {
            try
            {
                Thread.Sleep(2000);
                return InvalidUserCredentialsPopup.Displayed;
            }
            catch
            {
                return true;
            }
        }


        public bool InvalidBarcodeMessage()
        {
            Thread.Sleep(2000);
            return InvalidUserBarcodeMessagePopup.Displayed;
        }

        public bool IsAtLogin()
        {
            Driver.WaitForElementToBeDisplayed(StartShiftButton);
            return StartShiftButton.Displayed;
        }

        public bool IsAtCustomerLogin()
        {
            Driver.WaitForElementToBeDisplayed(CustomerAssertion);
            return CustomerAssertion.Displayed;
        }


        #endregion
    }
}
