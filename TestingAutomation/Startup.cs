﻿using Microsoft.Extensions.DependencyInjection;
using TestFramework.Configuration;
using TestingAutomation.Driver;
using TestingAutomation.Driver.Interfaces;


namespace TestFramework
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDriverFixture, DriverFixture>();
            services.AddScoped<IDriverType, DriverType>();
        }
    }
}
