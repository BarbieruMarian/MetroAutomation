using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium;
using TestFramework.Configuration;
using TestFramework.Selenium.Interfaces;
using TestFramework.Selenium.WebDriver;

namespace TestFramework.Test
{
    public abstract class BaseTest : IDisposable
    {
        protected IDriverProxy Driver;
        protected static ExtentTest test;
        protected static ExtentReports extent;
        protected readonly IDriverType browserDriverType;

        public BaseTest(IDriverType browserDriverType)
        {
            this.browserDriverType = browserDriverType;
            BeforeEach();
        }
        protected virtual void BeforeEach()
        {
            ConfigReader.InitializeSettings("Portugal");
            ExtentStart();
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
            extent.Flush();
            AfterEach();
        }

        private void ExtentStart()
        {
            extent = new ExtentReports();
            var htmlReporter = new ExtentV3HtmlReporter("C:/TestReports/TestReport" + DateTime.Now.ToString("_MMddyyyy_hhmmtt") + ".html");
            htmlReporter.Config.ReportName = DateTime.Now.ToString();

            extent.AttachReporter(htmlReporter); 
        }
    }
}
