using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoseAPI.Entities;
using System.Collections.Generic;
using System.Text.Json;

namespace RoseAPI.Controllers
{
    [Authorize]
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

        [AllowAnonymous]
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

        [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpPost]
        [Route("loginWithCredentialsViaDesktop")]
        public async Task<IActionResult> LoginWithCredentialsViaDesktop([FromForm] User user)
        {
            PasswordHasher<User> passwordHasher = new();
            var Shared = new Shared(this._context, this._config);
            var us = await Shared.Authenticate(user);
            if (us != null)
            {
                var token = Shared.GenerateToken(us);
                await _context.User.Where(eb => eb.Id == us.Id).ExecuteUpdateAsync(setters => setters.SetProperty(b => b.TemporaryData, "empty"));
                await _context.User.Where(eb => eb.Id == us.Id).ExecuteUpdateAsync(setters => setters.SetProperty(b => b.TemporaryData, token));
                return Ok(new { token, us });
            }

            return NotFound("user not found");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("loginWithMetaMask")]
        public async Task<IActionResult> LoginWithMetaMask([FromForm] User user)
        {
            var users = await _context.User.ToListAsync();
            var Shared = new Shared(this._context, this._config);
            User userToAdd = user;
            bool found = false;
            foreach (var us in users)
            {
                if (user.WalletAddress == us.WalletAddress)
                {
                    userToAdd = us;
                    found = true;
                }
            }
            if (found)
            {
                var token = Shared.GenerateToken(userToAdd);
                return Ok(new { token, userToAdd });
            }
            else
            {
                userToAdd.Id = Guid.NewGuid();
                await _context.User.AddAsync(userToAdd);
                await _context.SaveChangesAsync();
                var token = Shared.GenerateToken(userToAdd);
                return Ok(new { token, userToAdd });
            }
        }

        [HttpPost]
        [Route("getTemporaryData")]
        public async Task<IActionResult> GetTemporaryData([FromBody] object us)
        {
            JsonElement json = (JsonElement)us;
            User user = new User();
            var id = json.GetProperty("Id").ToString();
            var wa = json.GetProperty("WalletAddress").ToString();
            if (wa == "")
            {
                user = await _context.User.Where(eb => eb.Id.ToString() == id).FirstOrDefaultAsync();
                user.TemporaryData = "id";
            }
            if (id == "")
            {
                user = await _context.User.Where(eb => eb.WalletAddress.ToString() == wa).FirstOrDefaultAsync();
                user.TemporaryData = "wa";
            }
            return Ok(user);
        }
    }
}
