using OpenQA.Selenium;
using TestFramework.Configuration;
using TestFramework.Selenium.Interfaces;
using TestFramework.Selenium.WebDriver;

namespace TestFramework.Test
{
    public abstract class BaseTest : IDisposable
    {
        protected IDriverProxy Driver;
        protected readonly IDriverType browserDriverType;

        public BaseTest(IDriverType browserDriverType)
        {
            this.browserDriverType = browserDriverType;
            BeforeEach();
        }
        protected virtual void BeforeEach()
        {
            ConfigReader.InitializeSettings("Portugal");      
            Driver = new WebDriverProxy(GetWebDriver());
        }

        protected virtual void AfterEach()
        {
            Driver.Quit();
        }

        private IWebDriver GetWebDriver()
        {
            return Config.BrowserType switch
            {
                BrowserType.Chrome => browserDriverType.GetChromeDriver(),
                BrowserType.Firefox => browserDriverType.GetFirefoxDriver(),
                _ => browserDriverType.GetChromeDriver()
            };
        }

        public void Dispose()
        {
            AfterEach();
        }
    }
}
