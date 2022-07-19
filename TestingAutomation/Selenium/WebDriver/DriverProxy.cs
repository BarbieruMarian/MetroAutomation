using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TestFramework.Selenium.Interfaces;

namespace TestFramework.Selenium.WebDriver
{
    public abstract class DriverProxy<TDriver> : IDriverProxy
    {
        public TDriver Driver;
        public WebDriverWait Wait;
        public abstract string Url { get; }

        public abstract IWebElement FindElement(By by, double waitSeconds = 20);

        public abstract void Navigate(string url);

        public abstract void Quit();

        public abstract void Refresh();

        public abstract void SetLocalStorage();

        public abstract void WaitForElementToBeClickable(IWebElement element, int waitSeconds = 20);

        public abstract void WaitForElementToBeDisplayed(IWebElement element, int waitSeconds = 20);
    }
}
