namespace GameShop.DTO
{
    public class StickerCreateUpdateDto
    {
        public int GameId { get; set; }  
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = null!;
    }
}
