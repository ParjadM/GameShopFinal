using System.ComponentModel.DataAnnotations;

namespace GameShop.Models
{
    // Represents a playlist of games in the game shop
    public class Playlist
    {
        // Unique identifier for the playlist
        public int PlaylistID { get; set; }
        
        // Name of the playlist
        public string Name { get; set; } = null!;

        // Optional description of the playlist
        public string? Description { get; set; }

        // Date the playlist was created
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        // Relationship with games in the playlist
        public ICollection<PlaylistGame> PlaylistGames { get; set; } = new List<PlaylistGame>();
    }
}