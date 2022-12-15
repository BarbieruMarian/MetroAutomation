using TestFramework.Helper;
using TestFramework.Models;

namespace TestFramework.Contexts
{
    public class UsersContext : IUsersContext
    {
        private readonly Keycloak _keycloak;

        public UsersContext(Keycloak keycloak) =>
            _keycloak = keycloak;

        public async Task<User> CreateCashierAsync(int storeNumber, bool useTemporaryPassword, string password)
        {
            var keycloakUser = await _keycloak.CreateKeycloakUserAsync(storeNumber, Keycloak.UserRole.Cashier, useTemporaryPassword, password);
            var cashier = new Operator(keycloakUser);
            return new User(keycloakUser, cashier);
        }

        public User CreateCashier(int storeNumber, bool useTemporaryPassword, string password)
        {
            Task<User> task = Task.Run(async () => await CreateCashierAsync(storeNumber, useTemporaryPassword, password));
            return task.Result;
        }

        public async Task<User> CreateSupervisorAsync(int storeNumber, bool useTemporaryPassword, string password)
        {
            var keycloakUser = await _keycloak.CreateKeycloakUserAsync(storeNumber, Keycloak.UserRole.Supervisor, useTemporaryPassword, password);
            var cashier = new Operator(keycloakUser);
            return new User(keycloakUser, cashier);
        }

        public User CreateSupervisor(int storeNumber, bool useTemporaryPassword, string password)
        {
            Task<User> task = Task.Run(async () => await CreateSupervisorAsync(storeNumber, useTemporaryPassword, password));
            return task.Result;
        }

        public async Task DeleteUserAsync(User user) =>
            await _keycloak.DeleteKeyCloakUserAsync(user.KeycloakUser);

        public void DeleteUser(User user) =>
            Task.Run(async () => await DeleteUserAsync(user));
    }
}
