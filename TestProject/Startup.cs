using Microsoft.Extensions.DependencyInjection;
using TestFramework.Selenium.Interfaces;
using TestFramework.Selenium.WebDriver;

namespace TestProject
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDriverType, DriverType>();
        }
    }
}
