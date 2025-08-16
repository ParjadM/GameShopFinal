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
        private readonly IPlaylistService _playlistService;

        public ManageCatalogController(
            IGameService gameService,
            IStickerService stickerService,
            IPlaylistService playlistService) 
        {
            _gameService = gameService;
            _stickerService = stickerService;
            _playlistService = playlistService;
        }

        public async Task<IActionResult> Index()
        {
            var games = await _gameService.GetAllGamesAsync();
            var stickers = await _stickerService.GetAllAsync();
            var playlists = await _playlistService.GetAllPlaylistsWithGamesAsync();

            var model = new ManageCatalogViewModel
            {
                Games = games,
                Stickers = stickers,
                Playlists = playlists 
            };

            return View(model);
        }
    }
}
