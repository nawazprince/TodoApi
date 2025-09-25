using Microsoft.EntityFrameworkCore;
using TodoApi.DAL;
using TodoApi.Interfaces;
using TodoApi.Models;

namespace TodoApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<User> GetByEmailAsync(string email)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<bool> IsEmailExistAsync(string email)
        {
            return await _dbContext.Users
                                    .AnyAsync(u => u.Email == email);
        }
        public async Task AddAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}
