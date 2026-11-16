namespace vizsgaremek_backend.DTOs
{
    public class FriendDTO
    {
        public int FriendshipId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
