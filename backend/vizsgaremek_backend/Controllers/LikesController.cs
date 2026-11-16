using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vizsgaremek_backend.DTOs;
using vizsgaremek_backend.Models;

namespace vizsgaremek_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        private readonly VizsgaremekAbV1Context _context;

        public LikesController(VizsgaremekAbV1Context context)
        {
            _context = context;
        }

        [HttpGet("GetByPost/{postId}")]
        public async Task<IActionResult> GetByPostAsync(int postId)
        {
            try
            {
                var result = await _context.PostLikes.Include(l => l.User).Where(l => l.PostId == postId).Select(l => new LikeDTO
                {
                    Id = l.Id,
                    PostId = l.PostId,
                    UserId = l.UserId,
                    Username = l.User.Username,
                    ProfilePic = l.User.ProfilePic,
                    CreatedAt = l.CreatedAt
                }).ToListAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("Post")]
        public async Task<IActionResult> PostLikesAsync([FromBody] LikeInputDTO ujLike)
        {
            try
            {
                var like = new PostLike
                {
                    PostId = ujLike.PostId,
                    UserId = ujLike.UserId
                };

                await _context.PostLikes.AddAsync(like);
                await _context.SaveChangesAsync();
                return Ok("Sikeres mentés");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("Delete/{postId}/{userId}")]
        public async Task<IActionResult> DeleteLikesAsync(int postId, int userId)
        {
            try
            {
                var toDelete = await _context.PostLikes.FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);
                if (toDelete == null)
                {
                    return NotFound("Hiányzó like");
                }

                _context.PostLikes.Remove(toDelete);
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
