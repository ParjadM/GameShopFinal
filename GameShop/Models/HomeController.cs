using Microsoft.AspNetCore.Mvc;

namespace GameShop.Models
{
    public class Playlist
    {
        public int PlayListId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public ICollection<PlaylistGame> PlaylistGames { get; set; }
    }
}
