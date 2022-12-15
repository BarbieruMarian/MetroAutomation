using TestFramework.Models;

namespace TestFramework.Contexts
{
    public interface IUsersContext
    {
        Task<User> CreateCashierAsync(int storeNumber, bool useTemporaryPassword, string password);
        User CreateCashier(int storeNumber, bool useTemporaryPassword, string password);
        Task<User> CreateSupervisorAsync(int storeNumber, bool useTemporaryPassword, string password);
        User CreateSupervisor(int storeNumber, bool useTemporaryPassword, string password);
        Task DeleteUserAsync(User user);
        void DeleteUser(User user);
    }
}
