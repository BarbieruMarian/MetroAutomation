using OpenQA.Selenium;
using TestingAutomation.Driver.Interfaces;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace TestingAutomation.Driver
{
    public class DriverType : IDriverType
    {
        public IWebDriver GetChromeDriver()
        {
            var options = new ChromeOptions();
            options.AddArgument("no-sandbox");

            return new ChromeDriver(options);
        }

        public IWebDriver GetFirefoxDriver()
        {
            new DriverManager().SetUpDriver(new FirefoxConfig());
            return new FirefoxDriver();
        }
    }
}
