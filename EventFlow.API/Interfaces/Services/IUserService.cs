using EventFlow.API.DTOs;

namespace EventFlow.API.Interfaces.Services
{
    public interface IUserService
    {
        Task<LoginResponseModel> loginUser(LoginDTO login);
        Task<CreateUserResponeModel> createUser(RegisterUserRequest user);
    }
}
