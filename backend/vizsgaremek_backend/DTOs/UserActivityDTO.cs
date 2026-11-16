namespace vizsgaremek_backend.DTOs
{
    public class UserActivityDTO
    {
        public List<FriendDTO> Friends { get; set; }
        public List<PostDTO> Posts { get; set; }
        public List<CommentDTO> Comments { get; set; }
    }
}
