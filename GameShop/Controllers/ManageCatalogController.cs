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
        /// <summary>
        ///Initializes a new instance of the ManageCatalogController.
        ///</summary>
        ///<param name="gameService">The service for managing game data.</param>
        /// <param name="stickerService">The service for managing sticker data.</param>
        ///<param name="playlistService">The service for managing playlist data.</param>
        public ManageCatalogController(
            IGameService gameService,
            IStickerService stickerService,
            IPlaylistService playlistService) 
        {
            _gameService = gameService;
            _stickerService = stickerService;
            _playlistService = playlistService;
        }
        /// <summary>
        ///Renders the main catalog management page. It aggregates and displays lists of all games, stickers, and playlists.
        ///</summary>
        ///<returns>
        ///A task that represents the asynchronous operation.>
        ///that renders the index view with a composite model containing games, stickers, and playlists.
        /// </returns>
        ///<example>GET: /ManageCatalog</example>
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
