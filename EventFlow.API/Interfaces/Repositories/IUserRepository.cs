using EventFlow.API.Domain;

namespace EventFlow.API.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> addUser(User user);
        Task<List<User>> GetUsers();
        Task<User> getUser(string userId);
        Task<User> updateUser(User user);
        Task<bool> deleteUser(string userId);
        Task<User> GetUserByEmail(string email);
    }
}
