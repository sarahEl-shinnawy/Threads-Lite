using Microsoft.AspNetCore.Mvc;
using threadslite.API.Data;
using threadslite.API.Models;
using threadslite.API.Dtos;
using System.Security.Cryptography;
using System.Text;

namespace threadslite.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CreateUser(UserCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("Password is required");

            CreatePasswordHash(dto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email, // Ensure this property exists in both User and UserCreateDto
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(new { user.Id, user.Username });
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
}
