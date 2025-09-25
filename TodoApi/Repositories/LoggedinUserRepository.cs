using TodoApi.Interfaces;
using TodoApi.Models;

namespace TodoApi.Repositories
{
    public class LoggedinUserRepository : ILoggedinUserRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoggedinUserRepository(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<User> GetLoggedinUser()
        {
            User user = new User();

            await Task.Run(() =>
            {
                var userIdString = _httpContextAccessor.HttpContext?.User.FindFirst("UserId")?.Value;
                var userName = _httpContextAccessor.HttpContext?.User.FindFirst("UserName")?.Value;
                var email = _httpContextAccessor.HttpContext?.User.FindFirst("Email")?.Value;

                int userId = Convert.ToInt32(userIdString);

                user = new User { UserId = userId, UserName = userName, Email = email };

            });

            return user;
        }
    }
}
