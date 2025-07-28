using Microsoft.AspNetCore.Mvc;

namespace GameShop.DTO
{
    public class PlaylistDto
    {
        public int PlayListId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<GameDto> Games { get; set; } = new List<GameDto>();
    }
}
