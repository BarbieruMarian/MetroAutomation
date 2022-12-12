using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using TestFramework.Configuration;
using TestFramework.Helper;
using TestFramework.Selenium.Helpers;

namespace TestFramework.Selenium.WebDriver
{
    public class WebDriverProxy : DriverProxy<IWebDriver>
    {
        public override string Url => Driver.Url;

        public WebDriverProxy(IWebDriver driver)
        {
            Driver = driver;
            Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(Config.PageLoadImplicitWait);
            Driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(Config.ImplicitWait);
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Config.ImplicitWait);
            Driver.Manage().Window.Maximize();
            Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Config.ImplicitWait));
            JSProxy = new JSProxy(Driver, Wait);
            ElementMapping.ElementSelectors.Add(this, new Dictionary<IWebElement, By>());        
        }

        public override void JSClick(IWebElement element)
        {
            JSProxy.ExecuteScript("arguments[0].click();", element);
        }

        public override void JSClick(IWebElement element, double waitInSeconds = 20)
        {
            var oldTimeout = Driver.Manage().Timeouts().ImplicitWait;
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(waitInSeconds);

            try
            {
                Wait.Until(driver => element.Enabled);
                JSClick(element);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format($"Element {element.TagName} is not loaded. See Inner Exception:", ex.InnerException));
            }

            Driver.Manage().Timeouts().ImplicitWait = oldTimeout;
        }

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
                //TODO: after logging is implemented in MAIR-914, write the exception to the logs
            }

            result = Driver.FindElement(by);
            if (result != null)
            {
                ElementMapping.ElementSelectors[this][result] = by;
            }
            return result;
        }

        public override IList<IWebElement> GetAllElements(By by, bool ignoreException = true, int waitInSeconds = 15)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(waitInSeconds));
            IList<IWebElement> results = null;
            try
            {
                if (waitInSeconds > 0)
                {
                    wait.Until(driver => driver.FindElements(by).Count > 0);
                    results = Driver.FindElements(by);
                    if (results != null)
                    {
                        foreach (var elem in results)
                        {
                            ElementMapping.ElementSelectors[this][elem] = by;
                        }
                    }
                    return results;
                }
            }
            catch (Exception ex)
            {
                if (ignoreException == false)
                {
                    throw new Exception($"Elements are not found for {by}. See inner exception: ", ex.InnerException);
                }
                else return Array.Empty<IWebElement>();
            }

            results = Driver.FindElements(by);
            if (results != null)
            {
                foreach (var elem in results)
                {
                    ElementMapping.ElementSelectors[this][elem] = by;
                }
            }
            return results;
        }

        public override void WaitForElementToBeClickable(IWebElement element, int waitSeconds = 20)
        {
            Thread.Sleep(2000);
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

        public override void MoveToElement(IWebElement element)
        {
            Actions action = new Actions(Driver);
            action.MoveToElement(element);
            action.Perform();
        }
        public override void DoubleClick(IWebElement element)
        {
            Actions action = new Actions(Driver);
            action.DoubleClick(element); 
            action.Perform();
        }

        public override void ScrollToTheBottomOfThePage()
        {
            JSProxy.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
        }

        public override void ScrollToTheBottomInsideTheViewPage(IWebElement element)
        {
            JSProxy.ExecuteScript("arguments[0].scrollIntoView(true);", element);
        }

        //refactor this to work in paralel
        public override void SetLocalStorage()
        {
            JSProxy.ExecuteScript("localStorage.setItem('workstationId','TESTWSTID001850');");
            Driver.Navigate().Refresh();
        }

        public override void ResetCookies()
        {
            Driver.Manage().Cookies.DeleteAllCookies();
            ClearCache();
        }

        public override void ClearCache()
        {
            JSProxy.ExecuteScript("window.localStorage.clear()");
            JSProxy.ExecuteScript("window.sessionStorage.clear()"); 
            JSProxy.ExecuteScript("window.cookieStore.delete()"); 
        }

        public override void Refresh()
        {
            Driver.Navigate().Refresh();
        }

        public override void TabOpen()
        {
            Driver.SwitchTo().NewWindow(WindowType.Tab);
        }
        public override void TabClose()
        {
            Driver.Close();
        }

        public override void TabSwitchToFirst()
        {
            List<string> tabs = new List<string>(Driver.WindowHandles);
            Driver.SwitchTo().Window(tabs[0]);
        }

        public override void TabSwitchToNext()
        {
            List<string> tabs = new List<string>(Driver.WindowHandles);
            int currentTabIndex = tabs.FindIndex(x => x == Driver.CurrentWindowHandle);
            if (tabs.Count - 1 == currentTabIndex)
            {
                Driver.SwitchTo().Window(tabs[0]);
            }
            else
                Driver.SwitchTo().Window(tabs[++currentTabIndex]);
        }

        public override void MaximizeWindow()
        {
            Driver.Manage().Window.Maximize();
        }

        public override void MinimizeWindow()
        {
            Driver.Manage().Window.Minimize();
        }
        public override void Navigate(string url)
        {
            Driver.Navigate().GoToUrl(url);
        }        
        public override void NavigateBack()
        {
            Driver.Navigate().Back();
        }

        public override void Quit()
        {
            Driver.Quit();
        }

        public override void SaveScreenshot(string screenshotPath)
        {
            Screenshot ss = ((ITakesScreenshot)Driver).GetScreenshot();
            ss.SaveAsFile(screenshotPath,
            ScreenshotImageFormat.Png);
        }
    }
}
