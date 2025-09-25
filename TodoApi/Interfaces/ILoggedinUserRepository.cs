using TodoApi.Models;

namespace TodoApi.Interfaces
{
    public interface ILoggedinUserRepository
    {
        Task<User> GetLoggedinUser();
    }
}