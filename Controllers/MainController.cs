using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoseAPI.Entities;
using System.Collections.Generic;

namespace RoseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly RoseDBContext _context;
        private readonly IConfiguration _config;
        public MainController(RoseDBContext context, IConfiguration configuration)
        {
            this._context = context;
            this._config = configuration;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromForm] User user)
        {
            PasswordHasher<User> passwordHasher = new();

            var users = await _context.User.ToListAsync();
            foreach (var us in users)
            {
                if (user.Username == us.Username || user.Email == us.Email)
                {
                    return NotFound("User already exists");
                }
            }
            User userToAdd = user;

            userToAdd.Id = Guid.NewGuid();
            userToAdd.HashedPassword = passwordHasher.HashPassword(userToAdd, userToAdd.HashedPassword!);
            await _context.User.AddAsync(userToAdd);
            await _context.SaveChangesAsync();
            bool finished = true;
            return Ok(new { finished });
        }

        [HttpPost]
        [Route("loginWithCredentials")]
        public async Task<IActionResult> LoginWithCredentials([FromForm] User user)
        {
            PasswordHasher<User> passwordHasher = new();
            var Shared = new Shared(this._context, this._config);
            var us = await Shared.Authenticate(user);
            if (us != null)
            {
                var token = Shared.GenerateToken(us);
                return Ok(new { token, us });
            }

            return NotFound("user not found");
        }

        [HttpPost]
        [Route("loginWithMetaMask")]
        public async Task<IActionResult> LoginWithMetaMask([FromForm] User user)
        {
            var users = await _context.User.ToListAsync();
            foreach (var us in users)
            {
                if (user.WalletAddress == us.WalletAddress)
                {
                    return Ok(new {us});
                }
            }
            User userToAdd = user;
            userToAdd.Id = Guid.NewGuid();
            await _context.User.AddAsync(userToAdd);
            await _context.SaveChangesAsync();

            return Ok(new {userToAdd});
        }
    }
}
