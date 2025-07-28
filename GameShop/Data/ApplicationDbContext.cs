using GameShop.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Game> Games { get; set; }
    public DbSet<Playlist> Playlists { get; set; }
    public DbSet<PlaylistGame> PlaylistGames { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
    }
}