using OpenQA.Selenium;

namespace TestingAutomation.Driver.Interfaces
{
    public interface IDriverType
    {
        IWebDriver GetChromeDriver();
        IWebDriver GetFirefoxDriver();
    }
}
