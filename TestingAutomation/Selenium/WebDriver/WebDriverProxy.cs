using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using TestFramework.Helper;

namespace TestFramework.Selenium.WebDriver
{
    public class WebDriverProxy : DriverProxy<IWebDriver>
    {
        public WebDriverProxy(IWebDriver driver)
        {
            Driver = driver;
            Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(120);
            Driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(20);
            Driver.Manage().Window.Maximize();
            Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            ElementMapping.ElementSelectors.Add(this, new Dictionary<IWebElement, By>());
            
        }

        public override string Url => Driver.Url;

        public override IWebElement FindElement(By by, double waitSeconds = 20)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(waitSeconds));
            IWebElement result = null;
            try
            {
                if (waitSeconds > 0)
                {
                    result = wait.Until(drv => drv.FindElement(by));
                    if (result != null)
                    {
                        ElementMapping.ElementSelectors[this][result] = by;
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format($"Element not found for {by}. See Inner Exception:", ex.InnerException));
            }

            result = Driver.FindElement(by);
            if (result != null)
            {
                ElementMapping.ElementSelectors[this][result] = by;
            }
            return result;
        }

        public override void Navigate(string url)
        {
            Driver.Navigate().GoToUrl(url);
        }

        public override void Quit()
        {
            Driver.Quit();
        }

        public override void Refresh()
        {
            Driver.Navigate().Refresh();
        }

        public override void SetLocalStorage()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
            js.ExecuteScript("localStorage.setItem('workstationId','TESTWSTID001850');");
            Driver.Navigate().Refresh();
        }

        public override void WaitForElementToBeClickable(IWebElement element, int waitSeconds = 20)
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(waitSeconds));
            wait.IgnoreExceptionTypes(
                typeof(NoSuchElementException),
                typeof(ElementNotVisibleException),
                typeof(NoSuchWindowException),
                typeof(TimeoutException));

            wait.Until(ExpectedConditions.ElementToBeClickable(element));
        }

        public override void WaitForElementToBeDisplayed(IWebElement element, int waitSeconds = 20)
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(waitSeconds));
            try
            {
                wait.Until(drv => element != null && element.Displayed == true);
            }
            catch
            {

            }
        }
    }
}
