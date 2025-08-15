using GameShop.Services;
using GameShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GameShop.Controllers
{
    public class ManageCatalogController : Controller
    {
        private readonly IGameService _gameService;
        private readonly IStickerService _stickerService;

        public ManageCatalogController(IGameService gameService, IStickerService stickerService)
        {
            _gameService = gameService;
            _stickerService = stickerService;
        }

        public async Task<IActionResult> Index()
        {
            var games = await _gameService.GetAllGamesAsync();
            var stickers = await _stickerService.GetAllAsync();

            var model = new ManageCatalogViewModel
            {
                Games = games,
                Stickers = stickers
            };

            return View(model);
        }
    }
}
