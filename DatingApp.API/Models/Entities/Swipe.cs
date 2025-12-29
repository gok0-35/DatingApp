namespace DatingApp.API.Models.Entities;

public class Swipe : BaseEntity
{
    public int SwiperId { get; set; }
    public User Swiper { get; set; } = null!;

    public int SwipedUserId { get; set; }
    public User SwipedUser { get; set; } = null!;

    public bool IsLike { get; set; } // true = like, false = pass
}