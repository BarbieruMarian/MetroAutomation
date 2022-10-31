using System;
using TestFramework.Configuration;

namespace TestFramework.Contexts
{
    public class EnvironmentContext : IEnvironmentContext
    {
        public ContextConfig Config { get; }
        public KeycloakConfig KeycloakConfig { get; }

        public EnvironmentContext()
        {
            Config = new ContextConfig();
            KeycloakConfig = new KeycloakConfig(
                $"http://keycloak.{GetServer()}",
                Config.Get("keycloak:adminUserName"),
                Config.Get("keycloak:adminPassword"));
        }

        public string GetBasePathUrl() => $"https://ui.{GetServer()}";
        public string GetCloseShiftUri(string shiftId) => $"https://mposair-api-{GetEnvironmentType()}.{GetServer()}/shifts/{shiftId}/close";

        public string GetServer() => Config.Get("environment:server");

        public string GetShiftUri(string userId) => $"https://mposair-api-{GetEnvironmentType()}.{GetServer()}/shifts?userId={userId}&shiftStatus=Open";

        public int GetStoreNumber() => Convert.ToInt32(Config.Get("environment:storeNumber"));

        public string GetEnvironmentType() => Config.Get("environment:environmentType");
    }
}
