using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vizsgaremek_backend.DTOs;
using vizsgaremek_backend.Models;

namespace vizsgaremek_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly VizsgaremekAbV1Context _context;

        public CommentsController(VizsgaremekAbV1Context context)
        {
            _context = context;
        }

        [HttpGet("GetByPost/{postId}")]
        public async Task<IActionResult> GetByPostAsync(int postId)
        {
            try
            {
                var result = await _context.Comments.Include(c => c.User).Where(c => c.PostId == postId).Select(c => new CommentDTO
                {
                    Id = c.Id,
                    PostId = c.PostId,
                    UserId = c.UserId,
                    Username = c.User.Username,
                    ProfilePic = c.User.ProfilePic,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt
                }).ToListAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetByUser/{userId}")]
        public async Task<IActionResult> GetByUserAsync(int userId)
        {
            try
            {
                var result = await _context.Comments.Include(c => c.User).Where(c => c.UserId == userId).Select(c => new CommentDTO
                {
                    Id = c.Id,
                    PostId = c.PostId,
                    UserId = c.UserId,
                    Username = c.User.Username,
                    ProfilePic = c.User.ProfilePic,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt
                }).ToListAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("Post")]
        public async Task<IActionResult> PostCommentsAsync([FromBody] CommentInputDTO ujComment)
        {
            try
            {
                var comment = new Comment
                {
                    PostId = ujComment.PostId,
                    UserId = ujComment.UserId,
                    Content = ujComment.Content
                };

                await _context.Comments.AddAsync(comment);
                await _context.SaveChangesAsync();
                return Ok("Sikeres mentés");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("Put/{id}")]
        public async Task<IActionResult> PutCommentsAsync([FromBody] CommentInputDTO ujComment, int id)
        {
            try
            {
                var toUpdate = await _context.Comments.FindAsync(id);
                if (toUpdate == null)
                {
                    return NotFound("Hiányzó komment");
                }

                _context.Entry(toUpdate).CurrentValues.SetValues(ujComment);
                await _context.SaveChangesAsync();
                return Ok("Sikeres mentés");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteCommentsAsync(int id)
        {
            try
            {
                var toDelete = await _context.Comments.FindAsync(id);
                if (toDelete == null)
                {
                    return NotFound("Hiányzó komment");
                }

                _context.Comments.Remove(toDelete);
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