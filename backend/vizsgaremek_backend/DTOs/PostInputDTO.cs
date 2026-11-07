namespace vizsgaremek_backend.DTOs
{
    public class PostInputDTO
    {
        public int UserId { get; set; }
        public string? Content { get; set; }
        public string? ImageUrl { get; set; }
    }
}
