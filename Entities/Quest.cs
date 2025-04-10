namespace RoseAPI.Entities
{
    public class Quest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime? Deadline { get; set; }
        public User Author { get; set; }
        public Guid AuthorId { get; set; }
    }
}
