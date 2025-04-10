namespace RoseAPI.Entities
{
    public class User
    {
        public Guid? Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? HashedPassword { get; set; }
        public string? WalletAddress { get; set; }
        public string? TemporaryData { get; set; }
        public List<Quest> Tasks { get; set; } = new List<Quest>();
    }
}
