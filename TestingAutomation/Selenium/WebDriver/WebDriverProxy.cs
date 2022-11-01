using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using TestFramework.Helper;

namespace TestFramework.Selenium.WebDriver
{
    public class WebDriverProxy : DriverProxy<IWebDriver>
    {
        public override string Url => Driver.Url;

        public WebDriverProxy(IWebDriver driver)
        {
            Driver = driver;
            Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(120);
            Driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(20);
            Driver.Manage().Window.Maximize();
            Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            ElementMapping.ElementSelectors.Add(this, new Dictionary<IWebElement, By>());        
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

        public override void ScrollToTheBottomOfThePage()
        {
            ((IJavaScriptExecutor)Driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
            //body.Sendkeys(Keys.End)
        }

        //refactor this to work in paralel
        public override void SetLocalStorage()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
            js.ExecuteScript("localStorage.setItem('workstationId','TESTWSTID001850');");
            Driver.Navigate().Refresh();
        }

        public override void Refresh()
        {
            Driver.Navigate().Refresh();
        }

        public override void OpenNewTab()
        {
            Driver.SwitchTo().NewWindow(WindowType.Tab);
        }

        public override void SwitchToFirstTab()
        {
            List<string> tabs = new List<string>(Driver.WindowHandles);
            Driver.SwitchTo().Window(tabs[0]);
        }

        public override void SwitchToNextTab()
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
