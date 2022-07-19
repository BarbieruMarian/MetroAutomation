using OpenQA.Selenium;

namespace TestFramework.Selenium.Interfaces
{
    public interface IDriverType
    {
        IWebDriver GetChromeDriver();
        IWebDriver GetFirefoxDriver();
    }
}
