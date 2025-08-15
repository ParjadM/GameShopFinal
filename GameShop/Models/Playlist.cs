using System.ComponentModel.DataAnnotations;

namespace GameShop.Models
{
    public class Playlist
    {
        public int PlaylistID { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public ICollection<PlaylistGame> PlaylistGames { get; set; } = new List<PlaylistGame>();
    }
}