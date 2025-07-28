using Microsoft.AspNetCore.Mvc;

namespace GameShop.Models
{
    public class Game
    {
        internal DateTime ReleaseDate;

        public int GameId { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }

        public ICollection<PlaylistGame> PlaylistGames { get; set; }
    }
}
