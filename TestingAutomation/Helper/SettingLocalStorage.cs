using OpenQA.Selenium;
using TestFramework.Selenium.Interfaces;

namespace TestFramework.Helper
{
    public class SettingLocalStorage
    {
        public static void Setup(IDriverProxy driver)
        {
            //IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            ////IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            //js.ExecuteScript("localStorage.setItem('workstationId','TESTWSTID001850');");
            //driver.Refresh();
            driver.SetLocalStorage();
        }
    }
}
