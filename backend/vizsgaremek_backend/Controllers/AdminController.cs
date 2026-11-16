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

        [HttpGet("UserActivity/{userId}")]
        public async Task<IActionResult> UserActivityAsync(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return NotFound("Hiányzó felhasználó");
                }
                var friends = await _context.Friendships.Include(f => f.Requester).Include(f => f.Addressee)
                    .Where(f => f.Status == "accepted" && (f.RequesterId == userId || f.AddresseeId == userId))
                    .Select(f => new FriendDTO
                    {
                        FriendshipId = f.Id,
                        UserId = f.RequesterId == userId ? f.AddresseeId : f.RequesterId,
                        Username = f.RequesterId == userId ? f.Addressee.Username : f.Requester.Username,
                        Email = f.RequesterId == userId ? f.Addressee.Email : f.Requester.Email,
                        CreatedAt = f.CreatedAt
                    }).ToListAsync();

                var posts = await _context.Posts.Include(p => p.User).Where(p => p.UserId == userId).Select(p => new PostDTO
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

                var comments = await _context.Comments.Include(c => c.User).Where(c => c.UserId == userId).Select(c => new CommentDTO
                {
                    Id = c.Id,
                    PostId = c.PostId,
                    UserId = c.UserId,
                    Username = c.User.Username,
                    ProfilePic = c.User.ProfilePic,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt
                }).ToListAsync();

                var result = new UserActivityDTO
                {
                    Friends = friends,
                    Posts = posts,
                    Comments = comments
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
