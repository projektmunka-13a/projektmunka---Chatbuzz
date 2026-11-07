namespace vizsgaremek_backend.DTOs
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string? ProfilePic { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
