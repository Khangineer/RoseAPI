using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoseAPI.Entities;

namespace RoseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViabilityController : ControllerBase
    {
        private readonly RoseDBContext _context;
        public ViabilityController(RoseDBContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("checkusername")]
        public async Task<IActionResult> CheckUsername([FromBody] User userCame)
        {
            var users = await _context.User.ToListAsync();
            foreach(User user in users)
            {
                if(user.Username == userCame.Username)
                {
                    return Ok(true);
                }
            }
            return Ok(false);
        }

        [HttpPost]
        [Route("checkemil")]
        public async Task<IActionResult> CheckEmail([FromBody] User userCame)
        {
            var users = await _context.User.ToListAsync();
            foreach (User user in users)
            {
                if (user.Email == userCame.Email)
                {
                    return Ok(true);
                }
            }
            return Ok(false);
        }
    }
}
