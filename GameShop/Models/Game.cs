using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace GameShop.Models
{
    public class Game
    {
        public int GameId { get; set; }

        public string Title { get; set; } = null!;

        public string Genre { get; set; } = null!;

        public decimal Price { get; set; }

        public DateTime ReleaseDate { get; set; }  

        // Relationship to Customer
        public int? CustomerId { get; set; }  
        public Customer Customer { get; set; } = null!;  

        public ICollection<PlaylistGame> PlaylistGames { get; set; } = new List<PlaylistGame>();

        public ICollection<Sticker>? Stickers { get; set; }

    }
}
