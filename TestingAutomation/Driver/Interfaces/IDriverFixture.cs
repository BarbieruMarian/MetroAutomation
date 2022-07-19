using OpenQA.Selenium;
using TestFramework.Configuration;
using TestFramework.Selenium.Interfaces;

namespace TestingAutomation.Driver.Interfaces
{
    public interface IDriverFixture
    {
        public IDriverProxy Driver { get; }
    }
}
