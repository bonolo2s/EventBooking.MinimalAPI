using EventFlow.API.Domain;
using EventFlow.API.DTOs;

namespace EventFlow.API.Interfaces.Services
{
    public interface IUserService
    {
        Task<LoginResponseModel> loginUser(LoginDTO login);
        Task<User> createUser(RegisterUserRequest user);
        bool LogOutUser();// using refresh tokens ill be back.
        Task<User> UpdateUser();
        bool DeleteUser(string id);
    }
}
