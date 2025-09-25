using TodoApi.Models;

namespace TodoApi.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task<bool> IsEmailExistAsync(string email);
        Task AddAsync(User user);
    }
}