using OpenQA.Selenium;

namespace TestFramework.Selenium.Interfaces
{
    public interface IDriverProxy
    {
        public string Url { get; }
        void Navigate(string url);
        void OpenNewTab();
        void MaximizeWindow();
        void MinimizeWindow();
        void SwitchToNextTab();
        void SwitchToFirstTab();
        void ScrollToTheBottomOfThePage();
        void MoveToElement(IWebElement element);
        void Refresh();
        void Quit();
        void WaitForElementToBeDisplayed(IWebElement element, int waitSeconds = 20);
        void WaitForElementToBeClickable(IWebElement element, int waitSeconds = 20);
        void SetLocalStorage();
        IWebElement FindElement(By by, double waitSeconds = 20);
        IList<IWebElement> GetAllElements(By by, bool ignoreException = true, int waitInSeconds = 15);
        void SaveScreenshot(string screenshotPath);
    }
}
