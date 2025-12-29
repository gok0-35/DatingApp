namespace DatingApp.API.Models.Entities;

public class Message : BaseEntity
{
    public int MatchId { get; set; }
    public Match Match { get; set; } = null!;

    public int SenderId { get; set; }
    public User Sender { get; set; } = null!;

    public string Content { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }
}