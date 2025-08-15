using GameShop.Dtos;
using GameShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameShop.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameService _gameService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public GameController(IGameService gameService, IWebHostEnvironment webHostEnvironment)
        {
            _gameService = gameService;
            _webHostEnvironment = webHostEnvironment;
        }



        // ADD GAME - GET
        [HttpGet]
        public IActionResult AddGame()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddGame(GameCreateUpdateDto gameDto, IFormFile imageFile)
        {
            Console.WriteLine("AddGame POST hit");
            Console.WriteLine($"Title: {gameDto.Title}, Genre: {gameDto.Genre}, Price: {gameDto.Price}");

            // Check model state
            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is invalid!");
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"Field: {state.Key}, Error: {error.ErrorMessage}");
                    }
                }

                // Return the same view with the model to show validation errors
                return View("AddGame", gameDto);
            }

            // Handle image file if uploaded
            if (imageFile != null && imageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/games");
                Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                gameDto.ImagePath = "/images/games/" + uniqueFileName;
                Console.WriteLine($"Image uploaded to {gameDto.ImagePath}");
            }

            // Debug: list all games BEFORE adding
            var gamesBefore = await _gameService.GetAllGamesAsync();
            Console.WriteLine($"Total games BEFORE add: {gamesBefore.Count()}");

            foreach (var g in gamesBefore)
            {
                Console.WriteLine($"GameId: {g.GameId}, Title: {g.Title}, Genre: {g.Genre}, ImagePath: {g.ImagePath}");
            }

            // Create the game
            await _gameService.CreateGameAsync(gameDto);
            Console.WriteLine("Game created.");

            // Debug: list all games AFTER adding
            var gamesAfter = await _gameService.GetAllGamesAsync();
            Console.WriteLine($"Total games AFTER add: {gamesAfter.Count()}");

            foreach (var g in gamesAfter)
            {
                Console.WriteLine($"GameId: {g.GameId}, Title: {g.Title}, Genre: {g.Genre}, ImagePath: {g.ImagePath}");
            }

            Console.WriteLine("Redirecting to ManageCatalog/Index");

            return RedirectToAction("Index", "ManageCatalog");
        }


        [HttpGet]

        public async Task<IActionResult> EditGame(int id)
        {
            var game = await _gameService.GetGameByIdAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            var gameDto = new GameCreateUpdateDto
            {
                GameId = game.GameId,
                Title = game.Title,
                Genre = game.Genre,
                Price = game.Price,
                ReleaseDate = game.ReleaseDate,
                ImagePath = game.ImagePath,
              
            };

            return View(gameDto); 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGame(int id, GameCreateUpdateDto gameDto, IFormFile imageFile)
        {
            Console.WriteLine($"Received form data - GameId: {gameDto.GameId}, ImagePath: {gameDto.ImagePath}, Title: {gameDto.Title}, Genre: {gameDto.Genre}, Price: {gameDto.Price}, ReleaseDate: {gameDto.ReleaseDate}, ImageFile: {(imageFile != null ? imageFile.FileName : "null")}");

            // Check for ID mismatch
            if (id != gameDto.GameId)
            {
                Console.WriteLine($"ID mismatch: URL ID {id}, Form GameId {gameDto.GameId}");
                ModelState.AddModelError("GameId", "Game ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState invalid. Errors:");
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"Field: {state.Key}, Error: {error.ErrorMessage}");
                    }
                }

                // Preserve ImagePath on validation failure
                var existingGameForValidation = await _gameService.GetGameByIdAsync(id);
                if (existingGameForValidation != null)
                {
                    gameDto.ImagePath = existingGameForValidation.ImagePath;
                    Console.WriteLine($"Preserved ImagePath on validation failure: {gameDto.ImagePath}");
                }

                return View(gameDto);
            }

            var existingGame = await _gameService.GetGameByIdAsync(id);
            if (existingGame == null)
            {
                Console.WriteLine($"Game not found for ID: {id}");
                return NotFound();
            }

            Console.WriteLine($"Existing game ImagePath: {existingGame.ImagePath}");

            if (imageFile != null && imageFile.Length > 0)
            {
                // Delete old image if it exists
                if (!string.IsNullOrEmpty(existingGame.ImagePath))
                {
                    string oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, existingGame.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                        Console.WriteLine($"Deleted old image: {oldFilePath}");
                    }
                }

                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/games");
                Directory.CreateDirectory(uploadsFolder);
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                gameDto.ImagePath = "/images/games/" + uniqueFileName;
                Console.WriteLine($"New image uploaded: {gameDto.ImagePath}");
            }

            Console.WriteLine($"Final gameDto.ImagePath: {gameDto.ImagePath}");
            var updateResult = await _gameService.UpdateGameAsync(id, gameDto);
            if (!updateResult)
            {
                Console.WriteLine($"UpdateGameAsync failed for GameId: {id}");
                // Preserve ImagePath on update failure
                gameDto.ImagePath = existingGame.ImagePath;
                Console.WriteLine($"Preserved ImagePath on update failure: {gameDto.ImagePath}");
                return View(gameDto);
            }

            Console.WriteLine("Game updated successfully, redirecting to ManageCatalog/Index");
            return RedirectToAction("Index", "ManageCatalog");
        }

        // DELETE GAME - POST (for modal)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _gameService.DeleteGameAsync(id);
            if (!success)
            {
                // Optionally handle the case where deletion failed
                return BadRequest();
            }
            return RedirectToAction("Index", "ManageCatalog");
        }

    }
}
