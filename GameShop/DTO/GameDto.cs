using Microsoft.AspNetCore.Mvc;

namespace GameShop.DTO
{
    public class GameDto
    {
        public int GameId { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }

        public decimal Price { get; set; }         
        public DateTime ReleaseDate { get; set; }

        public string? ImagePath { get; set; }

        public List<StickerDto> Stickers { get; set; } = new();
        public string? ImageUrl { get; internal set; }
    }
}
