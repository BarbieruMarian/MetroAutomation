namespace TestFramework.Configuration
{
    public class KeycloakConfig
    {
        public string URL { get; }
        public string AdminUserName { get; }
        public string AdminPassword { get; }

        public KeycloakConfig(string url, string adminUserName, string adminPassword)
        {
            URL = url;
            AdminUserName = adminUserName;
            AdminPassword = adminPassword;
        }
    }
}
