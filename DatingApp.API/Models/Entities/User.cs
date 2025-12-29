using Microsoft.AspNetCore.Identity;

namespace DatingApp.API.Models.Entities;

public class User : IdentityUser<int>
{
    public string Name { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastActive { get; set; }

    // Navigation Properties
    public ICollection<Photo> Photos { get; set; } = new List<Photo>();
    public ICollection<Swipe> SwipesMade { get; set; } = new List<Swipe>();
    public ICollection<Swipe> SwipesReceived { get; set; } = new List<Swipe>();
}