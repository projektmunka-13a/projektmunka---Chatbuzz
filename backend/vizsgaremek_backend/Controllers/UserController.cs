using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vizsgaremek_backend.DTOs;
using vizsgaremek_backend.Models;

namespace vizsgaremek_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly VizsgaremekAbV1Context _context;

        public UsersController(VizsgaremekAbV1Context context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var result = await _context.Users.Select(u => new UserDTO
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    ProfilePic = u.ProfilePic,
                    Bio = u.Bio,
                    IsAdmin = u.IsAdmin,
                    IsBanned = u.IsBanned,
                    CreatedAt = u.CreatedAt
                }).ToListAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetUserNevReszlet")]
        public async Task<IActionResult> GetUserNevReszletAsync(string nevReszlet)
        {
            try
            {
                var talalat = await _context.Users.Where(u => u.Username.Contains(nevReszlet)).Select(u => new UserDTO
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    ProfilePic = u.ProfilePic,
                    Bio = u.Bio,
                    IsAdmin = u.IsAdmin,
                    IsBanned = u.IsBanned,
                    CreatedAt = u.CreatedAt
                }).ToListAsync();

                if (!talalat.Any())
                {
                    return NotFound("Nem található felhasználó");
                }

                return Ok(talalat);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var talalat = await _context.Users.Where(u => u.Id == id).Select(u => new UserDTO
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    ProfilePic = u.ProfilePic,
                    Bio = u.Bio,
                    IsAdmin = u.IsAdmin,
                    IsBanned = u.IsBanned,
                    CreatedAt = u.CreatedAt
                }).FirstOrDefaultAsync();

                if (talalat == null)
                {
                    return NotFound("Nem található felhasználó ezzel az azonosítóval: " + id);
                }
                return Ok(talalat);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("Put/{id}")]
        public async Task<IActionResult> PutUsersAsync([FromBody] UserUpdateDTO ujAdatok, int id)
        {
            try
            {
                var toUpdate = await _context.Users.FindAsync(id);
                if (toUpdate == null)
                {
                    return NotFound("Hiányzó felhasználó");
                }

                _context.Entry(toUpdate).CurrentValues.SetValues(ujAdatok);
                await _context.SaveChangesAsync();
                return Ok("Sikeres mentés");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("Ban/{id}")]
        public async Task<IActionResult> BanAsync(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound("Hiányzó felhasználó");
                }

                user.IsBanned = true;
                await _context.SaveChangesAsync();
                return Ok("Felhasználó kitiltva");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("Unban/{id}")]
        public async Task<IActionResult> UnbanAsync(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound("Hiányzó felhasználó");
                }

                user.IsBanned = false;
                await _context.SaveChangesAsync();
                return Ok("Tiltás feloldva");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteUsersAsync(int id)
        {
            try
            {
                var toDelete = await _context.Users.FindAsync(id);
                if (toDelete == null)
                {
                    return NotFound("Hiányzó felhasználó");
                }

                _context.Users.Remove(toDelete);
                await _context.SaveChangesAsync();
                return Ok("Sikeres törlés");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}