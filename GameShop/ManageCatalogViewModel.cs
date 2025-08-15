using GameShop.DTO;
using System.Collections.Generic;

namespace GameShop.ViewModels
{
    public class ManageCatalogViewModel
    {
        // List of games to display in the catalog
        public IEnumerable<GameDto> Games { get; set; } = new List<GameDto>();

        // List of stickers to display/manage
        public IEnumerable<StickerDto> Stickers { get; set; } = new List<StickerDto>();
    }
}
