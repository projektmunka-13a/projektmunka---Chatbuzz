using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vizsgaremek_backend.DTOs;
using vizsgaremek_backend.Models;

namespace vizsgaremek_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly VizsgaremekAbV1Context _context;

        public PostsController(VizsgaremekAbV1Context context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var result = await _context.Posts.Include(p => p.User).Select(p => new PostDTO
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    Username = p.User.Username,
                    ProfilePic = p.User.ProfilePic,
                    Content = p.Content,
                    ImageUrl = p.ImageUrl,
                    CreatedAt = p.CreatedAt,
                    LikeCount = p.PostLikes.Count(),
                    CommentCount = p.Comments.Count()
                }).ToListAsync();

                return Ok(result);
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
                var talalat = await _context.Posts.Include(p => p.User).Where(p => p.Id == id).Select(p => new PostDTO
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    Username = p.User.Username,
                    ProfilePic = p.User.ProfilePic,
                    Content = p.Content,
                    ImageUrl = p.ImageUrl,
                    CreatedAt = p.CreatedAt,
                    LikeCount = p.PostLikes.Count(),
                    CommentCount = p.Comments.Count()
                }).FirstOrDefaultAsync();

                if (talalat == null)
                {
                    return NotFound("Nem található poszt ezzel az azonosítóval: " + id);
                }
                return Ok(talalat);
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
                var result = await _context.Posts.Include(p => p.User).Where(p => p.UserId == userId).Select(p => new PostDTO
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    Username = p.User.Username,
                    ProfilePic = p.User.ProfilePic,
                    Content = p.Content,
                    ImageUrl = p.ImageUrl,
                    CreatedAt = p.CreatedAt,
                    LikeCount = p.PostLikes.Count(),
                    CommentCount = p.Comments.Count()
                }).ToListAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("Post")]
        public async Task<IActionResult> PostPostsAsync([FromBody] PostInputDTO ujPost)
        {
            try
            {
                var post = new Post
                {
                    UserId = ujPost.UserId,
                    Content = ujPost.Content,
                    ImageUrl = ujPost.ImageUrl
                };

                await _context.Posts.AddAsync(post);
                await _context.SaveChangesAsync();
                return Ok("Sikeres mentés");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("Put/{id}")]
        public async Task<IActionResult> PutPostsAsync([FromBody] PostInputDTO ujPost, int id)
        {
            try
            {
                var toUpdate = await _context.Posts.FindAsync(id);
                if (toUpdate == null)
                {
                    return NotFound("Hiányzó poszt");
                }

                _context.Entry(toUpdate).CurrentValues.SetValues(ujPost);
                await _context.SaveChangesAsync();
                return Ok("Sikeres mentés");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeletePostsAsync(int id)
        {
            try
            {
                var toDelete = await _context.Posts.FindAsync(id);
                if (toDelete == null)
                {
                    return NotFound("Hiányzó poszt");
                }

                _context.Posts.Remove(toDelete);
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