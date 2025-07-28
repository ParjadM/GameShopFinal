using Microsoft.AspNetCore.Mvc;

namespace GameShop.Models
{
    public class PlaylistGame
    {
        public int PlaylistId { get; set; }
        public int GameId { get; set; }
        public Playlist Playlist { get; set; }
        public Game Game { get; set; }
    }
}
