using Microsoft.AspNetCore.Mvc;
using threadslite.API.Data;
using threadslite.API.Models;
using threadslite.API.Models.DTOs;
using System.Linq;

namespace threadslite.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly AppDbContext _context;

    public PostsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult CreatePost([FromBody] PostCreateDto dto)
    {
        var user = _context.Users.Find(dto.UserId);
        if (user == null) return NotFound("User not found");

        var post = new Post
        {
            Content = dto.Content,
            UserId = dto.UserId,
            User = user
        };

        _context.Posts.Add(post);
        _context.SaveChanges();

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
    public IActionResult GetPosts()
    {
        var posts = _context.Posts
            .Select(p => new PostReadDto
            {
                Id = p.Id,
                Content = p.Content,
                CreatedAt = p.CreatedAt,
                UserId = p.UserId,
                Username = p.User.Username
            })
            .ToList();

        return Ok(posts);
    }
}
