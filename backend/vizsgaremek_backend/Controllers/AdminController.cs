using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vizsgaremek_backend.DTOs;
using vizsgaremek_backend.Models;

namespace vizsgaremek_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly VizsgaremekAbV1Context _context;

        public AdminController(VizsgaremekAbV1Context context)
        {
            _context = context;
        }
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsersAsync()
        {
            try
            {
                var result = await _context.Users.Select(u => new AdminUserDTO
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    CreatedAt = u.CreatedAt
                }).ToListAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
