using TestFramework.Configuration;
using TestFramework.Selenium.Interfaces;
using TestFramework.Test;
using TestProject.Pages;
using Xunit;

namespace TestProject.Tests
{
    public class StartShift : BaseTest
    {
        public StartShift(IDriverType browserDriverType) : base(browserDriverType)
        {
        }

        [UnitTestUtilities.Attributes.CountryFact("ES")]
        public void Start_Shift_Valid_Credentials()
        {
            //Step 1: Navigate to client main page
            var loginPage = new Login(Driver);
            loginPage.GoTo();
            Assert.True(loginPage.IsAtLogin(), "The main page could not be loaded");

            //Step 2: Start the shift with a valid supervisor
            var loginWithSupervisorResult = loginPage.LoginWithSuperviser(Config.Superviser, "123456");
            Assert.True(loginWithSupervisorResult.success, loginWithSupervisorResult.message);

            //Step 3: Scan a customer
            Assert.True(loginPage.IsAtCustomerLogin(), "The customer login page could not be loaded");
            var loginWithCustomerResult = loginPage.LoginWithCustomer(Config.Customer);
            Assert.True(loginWithCustomerResult.success, loginWithCustomerResult.message);

            //Step 4: Arrive at basket main page with that supervisor / customer 
            var basket = new Basket(Driver);
            Assert.True(basket.IsAtBasket(), "Basket Main Page was not loaded. Login failed.");                         
        }

        [UnitTestUtilities.Attributes.CountryFact("ES")]
        public void Start_Shift_Invalid_Supervisor_Credentials()
        {
            //Step 1: Navigate to client main page
            var loginPage = new Login(Driver);
            loginPage.GoTo();
            Assert.True(loginPage.IsAtLogin(), "The main page could not be loaded");

            //Step 2: Start the shift with an invalid supervisor
            loginPage.LoginWithSuperviser("1111111", "123456");
            Assert.True(loginPage.InvalidBarcodeMessage(), "Since we use invalid supervisor credentials, the expected result is login failed");
        }

        [UnitTestUtilities.Attributes.CountryFact("ES")]
        public void Start_Shift_Invalid_Customer_Credentials()
        {
            //Step 1: Navigate to client main page
            var loginPage = new Login(Driver);
            loginPage.GoTo();
            Assert.True(loginPage.IsAtLogin(), "The main page could not be loaded");

            //Step 2: Start the shift with an invalid supervisor
            var loginWithSupervisorResult = loginPage.LoginWithSuperviser(Config.Superviser, "123456");
            Assert.True(loginWithSupervisorResult.success, loginWithSupervisorResult.message);

            //Step 3: Scan a customer
            loginPage.LoginWithCustomer("111111");
            Assert.True(loginPage.InvalidUserCredentials(), "Since we use invalid supervisor credentials, the expected result is login failed");
        }
    }
}
