using OpenQA.Selenium;
using TestFramework.Selenium.Interfaces;

namespace TestFramework.Helper
{
    public class LocalStorage
    {
        public static void Setup(IDriverProxy driver)
        {
            driver.SetLocalStorage();
        }
    }
}
