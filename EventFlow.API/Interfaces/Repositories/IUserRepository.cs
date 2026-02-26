using EventFlow.API.Domain;

namespace EventFlow.API.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> addUser(User user);
        Task<List<User>> GetUsers();
        Task<User> getUser(Guid userId);
        Task<User> updateUser(User user);
        Task<bool> deleteUser(Guid userId);
        Task<User> GetUserByEmail(string email);
    }
}
