using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using threadslite.API.Data;
using threadslite.API.Models;
using threadslite.API.Models.DTOs;
using System.Linq;
using System.Threading.Tasks;

namespace threadslite.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]  // This ensures all endpoints require authentication
    public class PostsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PostsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] PostCreateDto dto)
        {
            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null) return NotFound("User not found");

            var post = new Post
            {
                Content = dto.Content,
                UserId = dto.UserId,
                User = user
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            var postDto = new PostReadDto
            {
                Id = post.Id,
                Content = post.Content,
                CreatedAt = post.CreatedAt,
                UserId = post.UserId,
                Username = user.Username
            };

            return Ok(postDto);
        }

        [HttpGet]
        [AllowAnonymous]  // If you want to allow public read access, else remove this line
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _context.Posts
                .Include(p => p.User) // Include related user entity
                .Select(p => new PostReadDto
                {
                    Id = p.Id,
                    Content = p.Content,
                    CreatedAt = p.CreatedAt,
                    UserId = p.UserId,
                    Username = p.User.Username
                })
                .ToListAsync();

            return Ok(posts);
        }
    }
}
