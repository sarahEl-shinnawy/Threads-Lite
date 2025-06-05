using Microsoft.AspNetCore.Mvc;
using threadslite.API.Data;
using threadslite.API.Models;
using threadslite.API.Models.DTOs;

namespace threadslite.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly AppDbContext _context;
    public CommentsController(AppDbContext context) => _context = context;

    [HttpPost]
    public IActionResult CreateComment([FromBody] CommentCreateDto dto)
    {
        var user = _context.Users.Find(dto.UserId);
        var post = _context.Posts.Find(dto.PostId);

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
        _context.SaveChanges();

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
    public IActionResult GetComments()
    {
        var comments = _context.Comments
            .Select(c => new CommentReadDto
            {
                Id = c.Id,
                Content = c.Content,
                CreatedAt = c.CreatedAt,
                PostId = c.PostId,
                UserId = c.UserId,
                Username = c.User.Username
            })
            .ToList();

        return Ok(comments);
    }
}
