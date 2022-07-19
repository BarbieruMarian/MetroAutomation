using TestFramework.Selenium.Interfaces;

namespace TestProject.Pages.Base
{
    public  class BasePage
    {
        protected IDriverProxy Driver { get; set; }
        public BasePage(IDriverProxy driver)
        {
            Driver = driver;
        }
    }
}
