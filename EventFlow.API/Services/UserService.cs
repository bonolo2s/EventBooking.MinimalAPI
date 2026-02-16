using EventFlow.API.DTOs;
using EventFlow.API.Interfaces.Services;

namespace EventFlow.API.Services
{
    public class UserService : IUserService
    {
        public Task<CreateUserResponeModel> createUser(RegisterUserRequest user)
        {
            throw new NotImplementedException();
        }

        public Task<LoginResponseModel> loginUser(LoginDTO login)
        {
            throw new NotImplementedException();
        }
    }
}
