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

        public async Task<bool> deleteUser(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            var builder = _context.Users.Where(u => u.Id == user.Id );

            var rows = await builder.ExecuteUpdateAsync(u =>
                u.SetProperty(x => x.status, AdminStatus.Inactive)
            );

            return rows > 0;
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

        public async Task<User> updateUser(User user)
        {
            var builder = _context.Users.Where(u => u.Id == user.Id);

            builder.ExecuteUpdate(u => u
                .SetProperty(x => x.FullName, user.FullName)
                .SetProperty(x => x.Email, user.Email)
            );

            return await _context.Users.FindAsync(user.Id);
        }
    }
}
