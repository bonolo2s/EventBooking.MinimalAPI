using EventFlow.API.Domain;

namespace EventFlow.API.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> addUser(User user);
        Task<List<User>> GetUsers();
        Task<User> getUser(string userId);
        Task<User> updateUser(User user);
        bool deleteUser(string userId); // or should i perform soft deletes?
        Task<User> GetUserByEmail(string email);
    }
}
