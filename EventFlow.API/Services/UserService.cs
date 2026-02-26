using EventFlow.API.Domain;
using EventFlow.API.DTOs;
using EventFlow.API.Helpers;
using EventFlow.API.Interfaces.Repositories;
using EventFlow.API.Interfaces.Services;
using System.Threading.Tasks;

namespace EventFlow.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly HashPasssword _hashPasssword;
        private readonly IJwtService _jwtService;

        public UserService(IUserRepository userRepository, HashPasssword hashPasssword,IJwtService jwtService)
        {
            _userRepository = userRepository;
            _hashPasssword = hashPasssword;
            _jwtService = jwtService;
        }

        public Task<User> createUser(RegisterUserRequest user)
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

        public async Task<bool> DeleteUser(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("id is missing");

            var user = await _userRepository.getUser(id);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            return await _userRepository.deleteUser(id);
        }

        public async Task<LoginResponseModel> loginUser(LoginDTO login)
        {
            if (login == null)
                throw new ArgumentNullException("login is null");

            if (string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password))
                throw new ArgumentException("Please fill in all the required fields");

            var user = await _userRepository.GetUserByEmail(login.Email);
            if (user == null)
                throw new ArgumentNullException("user not found");

            string token = string.Empty;

            if (_hashPasssword.Verify(login.Password, user.Password))
                token = _jwtService.generateToken(user);
            else
                throw new ArgumentException("Passwords dont match");

            var response = new LoginResponseModel
            {
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(150)
            };

            return response;
        }

        public bool LogOutUser()
        {
            throw new NotImplementedException();//session expirations
        }

        public async Task<User> UpdateUser(UpdateUserDTO update)
        {
            if (update == null)
                throw new ArgumentNullException("Update is null");

            if (string.IsNullOrEmpty(update.Fullname) | string.IsNullOrEmpty(update.Email))
                throw new ArgumentException("please fill in all the required fields");

            var user = await _userRepository.getUser(update.Id);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            user.FullName = update.Fullname;
            user.Email = update.Email;

            return await _userRepository.updateUser(user);//

        }
    }
}
