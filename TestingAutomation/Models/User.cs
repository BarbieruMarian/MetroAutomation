using TestFramework.Helper;

namespace TestFramework.Models
{
    public class User
    {
        public Keycloak.KeycloakUser KeycloakUser { get; }
        public Operator Operator { get; }

        public User(Keycloak.KeycloakUser keycloakUser, Operator @operator)
        {
            KeycloakUser = keycloakUser;
            Operator = @operator;
        }
    }
}
