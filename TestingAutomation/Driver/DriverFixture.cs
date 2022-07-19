using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TestFramework.Configuration;
using TestFramework.Selenium.Interfaces;
using TestFramework.Selenium.WebDriver;
using TestingAutomation.Driver.Interfaces;

namespace TestingAutomation.Driver
{
    public class DriverFixture : IDriverFixture, IDisposable
    {
        private IDriverProxy driver;
        //private IWebDriver driver;
        private readonly IDriverType browserDriverType;

        public IDriverProxy Driver => driver;

        public DriverFixture(IDriverType browserDriverType)
        {
            ConfigReader.InitializeSettings("DemoBranch");
            this.browserDriverType = browserDriverType;
            //this.driver = GetWebDriver();
            this.driver = new WebDriverProxy(GetWebDriver());
        }

        private IWebDriver GetWebDriver()
        {
            return Config.BrowserType switch
            {
                BrowserType.Chrome  => browserDriverType.GetChromeDriver(),
                BrowserType.Firefox => browserDriverType.GetFirefoxDriver(),
                _ => browserDriverType.GetChromeDriver()
            };
        }

        public void Dispose()
        {
            driver.Quit();
        }
    }
}
