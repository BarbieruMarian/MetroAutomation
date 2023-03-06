using TestFramework.Configuration;
using TestFramework.Selenium.Interfaces;
using TestFramework.Test;
using TestProject.Pages;
using Xunit;

namespace TestProject.Tests
{
    public class Itemization : BaseTest
    {
        public Itemization(IDriverType browserDriverType) : base(browserDriverType)
        {
        }

        [UnitTestUtilities.Attributes.CountryFact("ES")]
        public void Add_Article_To_Basket()
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

            //Step 5: Add a item to basket
            var itemID = "100355";
            basket.AddItem(itemID);
            Assert.True(basket.ValidateItemWasAdded(itemID), $"The required item with ID: {itemID} was not added to basket");
        }

        [UnitTestUtilities.Attributes.CountryFact("ES")]
        public void Add_Multiple_Articles_To_Basket()
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

            //Step 5: Add a item to basket
            var itemID = "100355";
            basket.AddItem(itemID);
            Assert.True(basket.ValidateItemWasAdded(itemID), $"The required item with ID: {itemID} was not added to basket");

            //Step 5: Add a second item to basket
            basket.AddItem(itemID);
            Assert.True(basket.ValidateItemWasAdded(itemID), $"The required item with ID: {itemID} was not added to basket");
        }

        [UnitTestUtilities.Attributes.CountryFact("ES")]
        public void Repeat_Add_Article_To_Basket()
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

            //Step 5: Add a item to basket
            var itemID = "100355";
            basket.AddItem(itemID);
            Assert.True(basket.ValidateItemWasAdded(itemID), $"The required item with ID: {itemID} was not added to basket");

            //Step 6: Repeat adding of articles 3 times 
            basket.RepeatAddArticle(numberOfTimes: 3);
            Assert.True(basket.ValidateItemsWereAdded(itemID, expectedOccurences: 4), $"Could not found 4 expected occurences of added item in UI");
        }

        [UnitTestUtilities.Attributes.CountryFact("ES")]
        public void Remove_Article_From_Basket()
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

            //Step 5: Add a item to basket
            var itemID = "100355";
            basket.AddItem(itemID);
            Assert.True(basket.ValidateItemWasAdded(itemID), $"The required item with ID: {itemID} was not added to basket");

            //Step 6: Remove that item from basket
            var totalAmountBeforeRemovingItem = basket.GetTotalAmount();
            basket.RemoveItemFromBasket();
            var totalAmountAfterRemovingItem = basket.GetTotalAmount();
            Assert.True(totalAmountBeforeRemovingItem > totalAmountAfterRemovingItem, "Removing item from basket failed. " +
                $"Total amount before removing: {totalAmountBeforeRemovingItem}. Total amount after removing: {totalAmountAfterRemovingItem}");
        }

        [UnitTestUtilities.Attributes.CountryFact("ES")]
        public void Search_For_Article()
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

            //Step 5: Search for an item
            basket.SearchItem("item");
        }

        [UnitTestUtilities.Attributes.CountryFact("ES")]
        public void Check_Basket_Submenu_Functionality()
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

            //Step 5: Go to basket submenu from the basket page
            Assert.True(basket.IsBasketButtonPresent());
        }

        [UnitTestUtilities.Attributes.CountryFact("ES")]
        public void Check_Basket_Invoice_Submenu_Functionality()
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

            //Step 5: Go to invoice submenu from the basket page
            Assert.True(basket.IsInvoiceButtonPresent());
        }


        [UnitTestUtilities.Attributes.CountryFact("ES")]
        public void Check_Total_Number_Of_Articles()
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

            //Step 5: Insert 27 articles
            var itemID = "100355";
            basket.InsertNumberOfArticles(numberOfArticles: 27, articleToInsert: itemID);
            var numberOfArticles = basket.GetNumberOfArticles();
            Assert.True(numberOfArticles == 27, $"Expected number of articles was 27, but actual number of articles found in UI is: {numberOfArticles}");
        }

        [UnitTestUtilities.Attributes.CountryFact("ES")]
        public void Add_Discount_Coupon_For_Existing_Articles()
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

            //Step 5: Insert 27 articles
            var itemID = "100355";
            basket.InsertNumberOfArticles(numberOfArticles: 27, articleToInsert: itemID);
            var numberOfArticles = basket.GetNumberOfArticles();
            Assert.True(numberOfArticles == 27, $"Expected number of articles was 27, but actual number of articles found in UI is: {numberOfArticles}");

            //Step 6: Insert discount coupon
            var discountCoupon = "741852963";
            basket.AddDiscountCoupon(discountCoupon);
            //We will need to properly assert the discount in UI after UI is working properly 
        }

        [UnitTestUtilities.Attributes.CountryFact("ES")]
        public void Check_Keypad_Numpads_Functionality()
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

            //Step 5: Check that keypad numbers works by pressing the keys 2 and 7
            var expectedkeysPressed = "27";
            basket.UseKeypad();
            var actualKeysPressed = basket.GetBasketKeypadIntroducedValues();
            Assert.True(actualKeysPressed == expectedkeysPressed, $"Expected key pressed was {expectedkeysPressed}, but actual key pressed is: {actualKeysPressed}");
        }

        [UnitTestUtilities.Attributes.CountryFact("ES")]
        public void Check_Keypad_Erase_Character()
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

            //Step 5: Check that keypad numbers works by pressing the keys 2 and 7 and then deleting the 7 key
            var expectedkeysPressed = "2";
            basket.UseKeypad();
            basket.EraseLastKey();
            var actualKeysPressed = basket.GetBasketKeypadIntroducedValues();
            Assert.True(actualKeysPressed == expectedkeysPressed, $"Expected key pressed was {expectedkeysPressed}, but actual key pressed is: {actualKeysPressed}");
        }

        [UnitTestUtilities.Attributes.CountryFact("ES")]
        public void Check_Keypad_Cancel_Entered_Characters()
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

            //Step 5: Check that keypad numbers works by pressing the keys 2 and 7 and then pressing the C key to cancel
            var expectedkeysPressed = "";
            basket.UseKeypad();
            basket.CancelKeysEntered();
            var actualKeysPressed = basket.GetBasketKeypadIntroducedValues();
            Assert.True(actualKeysPressed == expectedkeysPressed, $"Expected key pressed was {expectedkeysPressed}, but actual key pressed is: {actualKeysPressed}");
        }
    }
}