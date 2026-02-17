using EventFlow.API.Domain;
using EventFlow.API.DTOs;

namespace EventFlow.API.Interfaces.Services
{
    public interface IUserService
    {
        Task<LoginResponseModel> loginUser(LoginDTO login);
        Task<User> createUser(RegisterUserRequest user);
    }
}
