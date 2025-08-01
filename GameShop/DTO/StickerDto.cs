namespace GameShop.DTO
{
    public class StickerDto
    {
        public int StickerId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = null!;

        public string? GameTitle { get; set; }
    }
}
