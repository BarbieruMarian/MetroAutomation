using OpenQA.Selenium;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using TestFramework.Selenium.Interfaces;

namespace TestFramework.Selenium.WebDriver
{
    public class DriverType : IDriverType
    {
        public IWebDriver GetChromeDriver()
        {
            var options = new ChromeOptions();
            options.AddArgument("no-sandbox");
            //options.AddArguments("headless");
            return new ChromeDriver(options);
        }

        public IWebDriver GetFirefoxDriver()
        {
            new DriverManager().SetUpDriver(new FirefoxConfig());
            return new FirefoxDriver();
        }
    }
}
