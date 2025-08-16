using Microsoft.AspNetCore.Mvc;

namespace GameShop.Models
{
    // Represents a many-to-many relationship between Playlists and Games
    public class PlaylistGame

    {
    // Unique identifiers for the Playlist and Game
        public int PlaylistId { get; set; }

        // Navigation property for the Playlist
        public int GameId { get; set; }

        // Navigation properties for the related Playlist and Game
        public Playlist Playlist { get; set; }
        public Game Game { get; set; }
    }
}
