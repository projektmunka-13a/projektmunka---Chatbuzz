using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vizsgaremek_backend.DTOs;
using vizsgaremek_backend.Models;

namespace vizsgaremek_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly VizsgaremekAbV1Context _context;

        public AuthController(VizsgaremekAbV1Context context)
        {
            _context = context;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDTO adat)
        {
            try
            {
                bool foglalt = await _context.Users.AnyAsync(u => u.Username == adat.Username || u.Email == adat.Email);
                if (foglalt)
                {
                    return Conflict("Ez a felhasználónév vagy email már foglalt");
                }

                var ujUser = new User
                {
                    Username = adat.Username,
                    Email = adat.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(adat.Password)
                };

                await _context.Users.AddAsync(ujUser);
                await _context.SaveChangesAsync();
                return Ok("Sikeres regisztráció");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO adat)
        {
            try
            {
                string azonosito = adat.LoginAzonosito.Trim().ToLower();

                var user = await _context.Users.FirstOrDefaultAsync(u =>
                    u.Username.ToLower() == azonosito ||
                    u.Email.ToLower() == azonosito);

                if (user == null || !BCrypt.Net.BCrypt.Verify(adat.Password, user.PasswordHash))
                {
                    return Unauthorized("Hibás felhasználónév/email cím vagy jelszó");
                }

                if (user.IsBanned)
                {
                    return StatusCode(403, "A fiók le van tiltva");
                }

                return Ok(new UserDTO
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    ProfilePic = user.ProfilePic,
                    Bio = user.Bio,
                    IsAdmin = user.IsAdmin,
                    IsBanned = user.IsBanned,
                    CreatedAt = user.CreatedAt
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}