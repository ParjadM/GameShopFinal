using System.ComponentModel.DataAnnotations;

namespace GameShop.Dtos
{
    public class GameCreateUpdateDto

    {

        public int GameId { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Genre is required")]
        public string Genre { get; set; } = null!;

        [Range(0, 999.99, ErrorMessage = "Price must be between 0 and 999.99")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Release date is required")]
        public DateTime ReleaseDate { get; set; }

        public string? ImagePath { get; set; }
    }

}