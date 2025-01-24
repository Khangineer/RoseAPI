using Microsoft.AspNetCore.Http;
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
        public MainController(RoseDBContext context)
        {
            this._context = context;
        }

        [HttpPost]
        [Route("grid")]
        public async Task<IActionResult> TestGettingData([FromBody] Dictionary<string, string> JsonCame)
        {
            return Ok(JsonCame);
        }

    }
}
