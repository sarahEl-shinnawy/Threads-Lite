namespace ThreadsLite.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public List<Post> Posts { get; set; } = new List<Post>();
    }
}
