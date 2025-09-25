using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoApi.Models;

namespace TodoApi.Services
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }

    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey key;
        public TokenService(IConfiguration config)
        {
            key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }
        public string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("UserId", user.UserId.ToString()),
                new Claim("UserName", user.UserName),
                new Claim("UserEmail", user.Email),
            };
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = cred
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
