using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DatingApp.API.Models.Entities;

namespace DatingApp.API.Data;

public class AppDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // DbSet'ler - Tablolar
    public DbSet<Photo> Photos { get; set; }
    public DbSet<Swipe> Swipes { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Identity tablolarını Türkçe isimlendirme (opsiyonel)
        builder.Entity<User>().ToTable("Users");
        builder.Entity<IdentityRole<int>>().ToTable("Roles");
        builder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
        builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
        builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
        builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
        builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");

        // Swipe - Self-referencing relationship
        builder.Entity<Swipe>()
            .HasOne(s => s.Swiper)
            .WithMany(u => u.SwipesMade)
            .HasForeignKey(s => s.SwiperId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Swipe>()
            .HasOne(s => s.SwipedUser)
            .WithMany(u => u.SwipesReceived)
            .HasForeignKey(s => s.SwipedUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Match - Self-referencing relationship
        builder.Entity<Match>()
            .HasOne(m => m.User1)
            .WithMany()
            .HasForeignKey(m => m.User1Id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Match>()
            .HasOne(m => m.User2)
            .WithMany()
            .HasForeignKey(m => m.User2Id)
            .OnDelete(DeleteBehavior.Restrict);

        // Unique Index - Aynı kişiye birden fazla swipe yapılmasın
        builder.Entity<Swipe>()
            .HasIndex(s => new { s.SwiperId, s.SwipedUserId })
            .IsUnique();

        // Unique Index - Aynı iki kişi birden fazla match olmasın
        builder.Entity<Match>()
            .HasIndex(m => new { m.User1Id, m.User2Id })
            .IsUnique();
    }
}