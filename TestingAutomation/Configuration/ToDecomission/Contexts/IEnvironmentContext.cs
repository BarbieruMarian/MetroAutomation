using TestFramework.Configuration.ToDecomission;

namespace TestFramework.Contexts
{
    public interface IEnvironmentContext
    {
        ContextConfig Config { get; }
        KeycloakConfig KeycloakConfig { get; }
        string GetEnvironmentType();
        int GetStoreNumber();
        string GetServer();

        string GetBasePathUrl();
        string GetShiftUri(string userId);
        string GetCloseShiftUri(string shiftId);
    }
}
