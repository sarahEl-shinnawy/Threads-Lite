using System;

public class CommentReadDto
{
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public int PostId { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; }
}
