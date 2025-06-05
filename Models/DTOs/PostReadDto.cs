using System;

namespace threadslite.API.Models.DTOs
{
    public class PostReadDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
    }
}
