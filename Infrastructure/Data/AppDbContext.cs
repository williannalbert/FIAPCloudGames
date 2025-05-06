using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Infrastructure.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Library> Libraries { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Promotion> Promotions { get; set; }
    public DbSet<GamePromotion> GamePromotions { get; set; }
    public DbSet<Wallet> Wallets { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>()
            .HasOne<ApplicationUser>()
            .WithOne(u => u.User)
            .HasForeignKey<User>(u => u.ApplicationUserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<User>()
            .HasIndex(u => u.ApplicationUserId)
            .IsUnique();

        builder.Entity<User>()
            .HasOne(u => u.Library)
            .WithOne(l => l.User)
            .HasForeignKey<Library>(l => l.UserId);

        builder.Entity<Sale>()
            .HasOne(s => s.Library)
            .WithMany(l => l.Sales)
            .HasForeignKey(s => s.LibraryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Sale>()
            .HasOne(s => s.Game)
            .WithMany()
            .HasForeignKey(s => s.GameId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Sale>()
            .HasOne(s => s.Game)
            .WithMany(g => g.Sales)
            .HasForeignKey(s => s.GameId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Sale>()
            .HasOne(s => s.Promotion)
            .WithMany(p => p.Sales)
            .HasForeignKey(s => s.PromotionId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<GamePromotion>()
            .HasKey(gp => new { gp.GameId, gp.PromotionId });

        builder.Entity<GamePromotion>()
            .HasOne(gp => gp.Game)
            .WithMany(g => g.GamePromotions)
            .HasForeignKey(gp => gp.GameId);

        builder.Entity<GamePromotion>()
            .HasOne(gp => gp.Promotion)
            .WithMany(p => p.GamePromotions)
            .HasForeignKey(gp => gp.PromotionId);
        
        builder.Entity<User>()
            .HasOne(u => u.Wallet)
            .WithOne(w => w.User)
            .HasForeignKey<Wallet>(w => w.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

