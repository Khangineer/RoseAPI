using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RoseAPI.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RoseAPI
{
    public class Shared
    {
        private readonly RoseDBContext _context;
        private readonly IConfiguration _configuration;
        public Shared(RoseDBContext _context, IConfiguration _configuration)
        {
            this._configuration = _configuration;
            this._context = _context;
        }

        public string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            };
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public async Task<User> Authenticate(User userLogin)
        {
            PasswordHasher<User> passwordHasher = new();
            var currentUser = await _context.User.FirstOrDefaultAsync(x => x.Email.ToLower() ==
                userLogin.Email.ToLower());
            if (currentUser != null)
            {
                if (passwordHasher.VerifyHashedPassword(userLogin, currentUser.HashedPassword, userLogin.HashedPassword) == PasswordVerificationResult.Success)
                {
                    return currentUser;
                }
            }
            return null;
        }
    }
}
