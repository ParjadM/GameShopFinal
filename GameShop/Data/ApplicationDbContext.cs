using GameShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameShop.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // App's entities
        public DbSet<Game> Games { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistGame> PlaylistGames { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Sticker> Stickers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Existing configurations
            modelBuilder.HasAnnotation("Relational:HistoryTable", "__efmigrationshistory");

            modelBuilder.Entity<PlaylistGame>()
                .HasKey(pg => new { pg.PlaylistId, pg.GameId });

            modelBuilder.Entity<PlaylistGame>()
                .HasOne(pg => pg.Playlist)
                .WithMany(p => p.PlaylistGames)
                .HasForeignKey(pg => pg.PlaylistId);

            modelBuilder.Entity<PlaylistGame>()
                .HasOne(pg => pg.Game)
                .WithMany(g => g.PlaylistGames)
                .HasForeignKey(pg => pg.GameId);

            modelBuilder.Entity<Game>()
                .HasOne(g => g.Customer)
                .WithMany(c => c.Games)
                .HasForeignKey(g => g.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Sticker>()
                .HasOne(s => s.Game)
                .WithMany(g => g.Stickers)
                .HasForeignKey(s => s.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            // Precision for decimal properties
            modelBuilder.Entity<Game>()
                .Property(g => g.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Sticker>()
                .Property(s => s.Price)
                .HasPrecision(18, 2);
        }


    }
}
