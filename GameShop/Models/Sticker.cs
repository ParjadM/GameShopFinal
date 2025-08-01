using System.ComponentModel.DataAnnotations;

namespace GameShop.Models
{
    public class Sticker
    {
        public int StickerId { get; set; }

        [Required]
        public int GameId { get; set; }



        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [StringLength(300)]
        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [StringLength(200)]
        public string? ImageUrl { get; set; }

        public Game? Game { get; set; }
    }

}
