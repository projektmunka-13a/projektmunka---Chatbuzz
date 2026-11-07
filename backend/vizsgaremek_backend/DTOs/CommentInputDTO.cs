namespace vizsgaremek_backend.DTOs
{
    public class CommentInputDTO
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
    }
}
