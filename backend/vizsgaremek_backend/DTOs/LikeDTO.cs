namespace vizsgaremek_backend.DTOs
{
    public class LikeDTO
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string? ProfilePic { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
