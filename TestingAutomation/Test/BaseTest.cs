using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Configuration;
using OpenQA.Selenium;
using TestFramework.Configuration;
using TestFramework.Selenium.Interfaces;
using TestFramework.Selenium.WebDriver;
using Xunit;
using Xunit.Abstractions;

namespace TestFramework.Test
{
    public abstract class BaseTest : IDisposable
    {
        protected IDriverProxy Driver;
        protected static ExtentTest test;
        protected static ExtentReports extent;
        protected readonly IDriverType browserDriverType;
        protected ITestOutputHelper output;

        public BaseTest(IDriverType browserDriverType)
        {
            this.browserDriverType = browserDriverType;
            output.WithReportPortal();
            BeforeEach();
        }
        protected virtual void BeforeEach()
        {
            ConfigReader.InitializeSettings();
            ExtentStart();
            Driver = new WebDriverProxy(GetWebDriver());

        }

        protected virtual void AfterEach()
        {
            extent.Flush();
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

        private void ExtentStart()
        {
            //extent = new ExtentReports(); 
            //var htmlReporter = new ExtentV3HtmlReporter("C:/TestReports/TestReport" + DateTime.Now.ToString("_MMddyyyy_hhmmtt") + ".html");
            //htmlReporter.Config.ReportName = DateTime.Now.ToString();
            //htmlReporter.Config.DocumentTitle = "Automation Report";
            //htmlReporter.Config.Theme = Theme.Dark;

            //extent.AttachReporter(htmlReporter);
        }
    }
}
