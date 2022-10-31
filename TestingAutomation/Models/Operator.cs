using TestFramework.Helper;


namespace TestFramework.Models
{
    public class Operator
    {
        public string OperatorId { get; set; }
        public string Pin { get; set; }
        public string Barcode { get; set; }

        public Operator(Keycloak.KeycloakUser keycloakUser)
        {
            OperatorId = keycloakUser.OperatorId;
            Pin = keycloakUser.Password;
            Barcode = keycloakUser.Barcode;
        }
    }
}
