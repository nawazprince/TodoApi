using Microsoft.EntityFrameworkCore;
using TodoApi.DAL;
using TodoApi.Interfaces;
using TodoApi.Repositories;
using TodoApi.Services;

namespace TodoApi.Extensions
{
    public static class ApplicationServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(Configuration.GetConnectionString("TodoApp")));

            services.AddHttpContextAccessor();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ILoggedinUserRepository, LoggedinUserRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITodoRepository, TodoRepository>();

            return services;
        }
    }
}
