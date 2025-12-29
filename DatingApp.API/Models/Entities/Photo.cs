namespace DatingApp.API.Models.Entities;

public class Photo : BaseEntity
{
    public string Url { get; set; } = string.Empty;
    public bool IsMain { get; set; }
    public string? PublicId { get; set; } // Cloudinary için

    // Foreign Key
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}