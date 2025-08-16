using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace GameShop.Models
{
    // Represents a game in the shop
    public class Game
    {
        // Unique identifier for the game
        public int GameId { get; set; }

        // Title of the game
        public string Title { get; set; } = null!;

        // Genre of the game
        public string Genre { get; set; } = null!;

        // Price of the game
        public decimal Price { get; set; }

        // Image path for the game
        public string? ImagePath { get; set; }

        // Release date of the game
        public DateTime ReleaseDate { get; set; }

        // Relationship to Customer
        public int? CustomerId { get; set; }

        // Navigation property for Customer
        public Customer? Customer { get; set; } = null!;

        // Relationship with playlists
        public ICollection<PlaylistGame> PlaylistGames { get; set; } = new List<PlaylistGame>();

        // Collection of stickers related to the game
        public ICollection<Sticker>? Stickers { get; set; }



    }
}
