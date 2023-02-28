using OpenQA.Selenium;

namespace TestFramework.Selenium.Interfaces
{
    public interface IDriverProxy
    {
        public string Url { get; }
        void JSClick(IWebElement element);
        void JSClick(IWebElement element, double waitInSeconds = 20);
        void Navigate(string url);
        void NavigateBack();
        #region Tab
        void TabOpen();
        void TabClose(); 
        void TabSwitchToNext();
        void TabSwitchToFirst();
        #endregion
        void MaximizeWindow();
        void MinimizeWindow();
        void ScrollToTheBottomOfThePage();
        void ScrollToTheBottomInsideTheViewPage(IWebElement element);
        void DoubleClick(IWebElement element);
        void MoveToElement(IWebElement element);
        void Refresh();
        void Quit();
        void WaitForElementToBeDisplayed(IWebElement element, int waitSeconds = 20);
        void WaitForElementToBeClickable(IWebElement element, int waitSeconds = 20);
        void SetLocalStorage();
        void ResetCookies();
        void ClearCache();
        IWebElement FindElement(By by, double waitSeconds = 20);
        IList<IWebElement> GetAllElements(By by, bool ignoreException = true, int waitInSeconds = 15);
        void SaveScreenshot(string screenshotPath);
        void ExecuteScript(string script, IWebElement element);
        void SetElementFromDisabledToEnabled(IWebElement element);
    }
}
