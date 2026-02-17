using EventFlow.API.Domain;
using EventFlow.API.Infrastructure.DataAcess;
using EventFlow.API.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EventFlow.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly EventFlowDbContext _context;

        public UserRepository(EventFlowDbContext context)
        {
            _context = context;
        }

        public async Task<User> addUser(User user)
        {

            var result = _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public bool deleteUser(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<User?> getUser(string userId)
        {
            var user =await  _context.Users.FindAsync(userId);
            return user;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var newUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return newUser;
        }

        public async Task<List<User>> GetUsers()
        {
            var users = new List<User>();
            users = await _context.Users.ToListAsync();

            return users;
        }

        public Task<User> updateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
