using GameShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameShop.Controllers
{
    public class ManageCatalogController : Controller
    {
        private readonly IGameService _gameService;

        public ManageCatalogController(IGameService gameService)
        {
            _gameService = gameService;
        }

        public async Task<IActionResult> Index()
        {
            var games = await _gameService.GetAllGamesAsync();
            return View(games); 
        }
    }
}
