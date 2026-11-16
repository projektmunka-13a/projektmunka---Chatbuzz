namespace vizsgaremek_backend.DTOs
{
    public class PostDetailsDTO
    {
        public List<CommentDTO> Comments { get; set; }
        public List<LikeDTO> Likes { get; set; }
    }
}
