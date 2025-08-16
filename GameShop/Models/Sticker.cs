using System.ComponentModel.DataAnnotations;

namespace GameShop.Models
{
    // Represents a sticker associated with a game in the game shop
    public class Sticker
    {
        // Unique identifier for the sticker
        public int StickerId { get; set; }
        // Unique identifier for the game this sticker belongs to
        [Required]
        public int GameId { get; set; }

        // Name of the sticker

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        // Optional description of the sticker

        [StringLength(300)]
        public string? Description { get; set; }

        // Price of the sticker

        [Required]
        public decimal Price { get; set; }

        // Optional image URL for the sticker

        [StringLength(200)]
        public string? ImageUrl { get; set; }
        
        // Navigation property for the game this sticker belongs to
        public Game? Game { get; set; }
    }

}
