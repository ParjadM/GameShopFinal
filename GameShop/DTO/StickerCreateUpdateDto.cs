using System.ComponentModel.DataAnnotations;

namespace GameShop.DTO
{
    public class StickerCreateUpdateDto
    {
        [Required(ErrorMessage = "Please enter a name.")]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Please enter a price.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Please select a game.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid game.")]
        public int GameId { get; set; }

        public string? ImageUrl { get; set; }
    }
}
