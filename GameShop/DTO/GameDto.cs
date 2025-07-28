using Microsoft.AspNetCore.Mvc;

namespace GameShop.DTO
{
    public class GameDto
    {
        public int GameId { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
    }
}
