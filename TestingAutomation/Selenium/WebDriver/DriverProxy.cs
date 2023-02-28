using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using TestFramework.Selenium.Helpers;
using TestFramework.Selenium.Interfaces;

namespace TestFramework.Selenium.WebDriver
{
    public abstract class DriverProxy<TDriver> : IDriverProxy
    {
        public TDriver Driver;
        public WebDriverWait Wait;
        public JSProxy JSProxy;
        public abstract string Url { get; }
        public abstract void JSClick(IWebElement element);
        public abstract void JSClick(IWebElement element, double waitInSeconds = 20);
        public abstract IWebElement FindElement(By by, double waitSeconds = 20);
        public abstract IList<IWebElement> GetAllElements(By by, bool ignoreException = true, int waitInSeconds = 15);
        public abstract void Navigate(string url);
        public abstract void NavigateBack();
        #region Tab
        public abstract void TabOpen(); 
        public abstract void TabClose();
        public abstract void TabSwitchToNext();
        public abstract void TabSwitchToFirst();
        #endregion
        public abstract void MaximizeWindow();
        public abstract void MinimizeWindow();
        public abstract void ScrollToTheBottomOfThePage();
        public abstract void ScrollToTheBottomInsideTheViewPage(IWebElement element);
        public abstract void MoveToElement(IWebElement element);
        public abstract void DoubleClick(IWebElement element);
        public abstract void Quit();
        public abstract void Refresh();
        public abstract void SetLocalStorage();
        public abstract void ResetCookies();
        public abstract void ClearCache();
        public abstract void WaitForElementToBeClickable(IWebElement element, int waitSeconds = 20);
        public abstract void WaitForElementToBeDisplayed(IWebElement element, int waitSeconds = 20);
        public abstract void SaveScreenshot(string screenshotPath);
        public abstract void ExecuteScript(string script, IWebElement element);
        public abstract void SetElementFromDisabledToEnabled(IWebElement element);
    }
}
