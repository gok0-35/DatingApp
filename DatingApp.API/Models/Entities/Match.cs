namespace DatingApp.API.Models.Entities;

public class Match : BaseEntity
{
    public int User1Id { get; set; }
    public User User1 { get; set; } = null!;

    public int User2Id { get; set; }
    public User User2 { get; set; } = null!;

    // Navigation Property
    public ICollection<Message> Messages { get; set; } = new List<Message>();
}