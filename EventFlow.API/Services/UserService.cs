using EventFlow.API.Domain;
using EventFlow.API.DTOs;
using EventFlow.API.Helpers;
using EventFlow.API.Interfaces.Repositories;
using EventFlow.API.Interfaces.Services;

namespace EventFlow.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly HashPasssword _hashPasssword;

        public UserService(IUserRepository userRepository, HashPasssword hashPasssword)
        {
            _userRepository = userRepository;
            _hashPasssword = hashPasssword;
        }

        public Task<User> createUser(RegisterUserRequest user)
        {
            try
            {
                if (user == null)
                    throw new ArgumentNullException("user cannot be null");
 
                if (string.IsNullOrEmpty(user.FullName) ||
                    string.IsNullOrEmpty(user.Email) || 
                    string.IsNullOrEmpty(user.Password))
                    throw new ArgumentNullException("Please fill in all the details");
                


                var newUser = new User
                {
                    FullName = user.FullName,
                    Email = user.Email,
                    Password = _hashPasssword.hash(user.Password),
                    Role = Role.Admin,// for now all user ill defualt to admin
                };

                var result = _userRepository.addUser(newUser);

                return result;
            }
            catch (Exception ex) {
                throw new Exception("An unexpected error occurred while creating the user", ex);
            }
        }

        public Task<LoginResponseModel> loginUser(LoginDTO login)
        {
            throw new NotImplementedException();
        }
    }
}
