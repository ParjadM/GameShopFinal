using System.ComponentModel.DataAnnotations;

namespace GameShop.Dtos
{
    public class GameCreateUpdateDto
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}