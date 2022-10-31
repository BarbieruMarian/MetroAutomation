using OpenQA.Selenium;

namespace TestFramework.Selenium.Interfaces
{
    public interface IDriverProxy
    {
        public string Url { get; }
        void Navigate(string url);
        void MaximizeWindow();
        void MinimizeWindow();
        void ScrollToTheBottomOfThePage();
        void MoveToElement(IWebElement element);
        void Refresh();
        void Quit();
        void WaitForElementToBeDisplayed(IWebElement element, int waitSeconds = 20);
        void WaitForElementToBeClickable(IWebElement element, int waitSeconds = 20);
        void SetLocalStorage();
        IWebElement FindElement(By by, double waitSeconds = 20);
    }
}
