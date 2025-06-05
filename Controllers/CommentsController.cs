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
    [Authorize] 
    public class CommentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CommentsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CommentCreateDto dto)
        {
            var user = await _context.Users.FindAsync(dto.UserId);
            var post = await _context.Posts.FindAsync(dto.PostId);

            if (user == null || post == null)
                return NotFound("User or Post not found");

            var comment = new Comment
            {
                Content = dto.Content,
                PostId = dto.PostId,
                UserId = dto.UserId,
                Post = post,
                User = user
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            var commentDto = new CommentReadDto
            {
                Id = comment.Id,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                PostId = comment.PostId,
                UserId = comment.UserId,
                Username = user.Username
            };

            return Ok(commentDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetComments()
        {
            var comments = await _context.Comments
                .Include(c => c.User)
                .Select(c => new CommentReadDto
                {
                    Id = c.Id,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    PostId = c.PostId,
                    UserId = c.UserId,
                    Username = c.User.Username
                })
                .ToListAsync();

            return Ok(comments);
        }
    }
}
