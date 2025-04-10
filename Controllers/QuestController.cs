using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RoseAPI.Entities;
using System.Text.Json;

namespace RoseAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class QuestController : ControllerBase
    {
        private RoseDBContext _context;

        public QuestController(RoseDBContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("getuserquests")]
        public async Task<IActionResult> GetUserQuests([FromBody] object user)
        {
            JsonElement userJ = (JsonElement)user;
            var id = userJ.GetProperty("Id").ToString();
            List<Quest> tasks = await _context.Quest.Where(eb => eb.AuthorId == Guid.Parse(id)).ToListAsync();
            return Ok(tasks);
        }
    }
}
