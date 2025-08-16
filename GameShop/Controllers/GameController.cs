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



        /// <summary>
        /// Renders the view that displays a form for adding a new game.
        /// </summary>
        /// <returns>Renders the 'Add Game' page.</returns>
        /// <example>
        /// GET: /Games/AddGame
        /// </example>
        [HttpGet]
        public IActionResult AddGame()
        {
            return View();
        }

        /// <summary>
        /// Handles the HTTP POST request to create a new game from the submitted form data,
        /// including an optional image upload for the game's cover.
        /// </summary>
        /// <param name="gameDto">The game data (title, genre, price) submitted from the form.</param>
        /// <param name="imageFile">The optional uploaded image file for the game. Can be null.</param>
        /// <returns>
        /// Redirects to the catalog management page on successful creation.
        /// If model validation fails, it re-displays the Add Game form with the user's submitted data and validation errors.
        /// </returns>
        /// <example>
        /// POST: /GameController/AddGame
        /// Body: (multipart/form-data containing gameDto fields and an imageFile)
        /// </example>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddGame(GameCreateUpdateDto gameDto, IFormFile imageFile)
        {
            Console.WriteLine("AddGame POST hit");
            Console.WriteLine($"Title: {gameDto.Title}, Genre: {gameDto.Genre}, Price: {gameDto.Price}");

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

                
                return View("AddGame", gameDto);
            }

            
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

            
            var gamesBefore = await _gameService.GetAllGamesAsync();
            Console.WriteLine($"Total games BEFORE add: {gamesBefore.Count()}");

            foreach (var g in gamesBefore)
            {
                Console.WriteLine($"GameId: {g.GameId}, Title: {g.Title}, Genre: {g.Genre}, ImagePath: {g.ImagePath}");
            }

            
            await _gameService.CreateGameAsync(gameDto);
            Console.WriteLine("Game created.");

            
            var gamesAfter = await _gameService.GetAllGamesAsync();
            Console.WriteLine($"Total games AFTER add: {gamesAfter.Count()}");

            foreach (var g in gamesAfter)
            {
                Console.WriteLine($"GameId: {g.GameId}, Title: {g.Title}, Genre: {g.Genre}, ImagePath: {g.ImagePath}");
            }

            Console.WriteLine("Redirecting to ManageCatalog/Index");

            return RedirectToAction("Index", "ManageCatalog");
        }



        /// <summary>
        /// Renders the view for editing the details of an existing game.
        /// </summary>
        /// <param name="id">The unique identifier of the game to be edited.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> that displays the edit form populated with the game's data.
        /// Returns a (HTTP 404) if the game is not found.
        /// </returns>
        /// <example>
        /// GET: /GameController/EditGame/5
        /// </example>
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

        /// <summary>
        /// Handles the HTTP POST request to update an existing game with submitted form data.
        /// This includes processing an optional new image file, which will replace the existing one.
        /// </summary>
        /// <param name="id">The unique identifier of the game being updated, from the URL.</param>
        /// <param name="gameDto">The updated game data submitted from the edit form.</param>
        /// <param name="imageFile">The optional new image file. If provided, it replaces the current game image.</param>
        /// <returns>
        /// to the catalog management page on successful update.
        /// The Edit view with validation errors if the submitted data is invalid.
        /// A (HTTP 404) if the game to update is not found or if the URL ID mismatches the form ID.
        /// </returns>
        /// <example>
        /// POST: /GameController/EditGame/5
        /// </example>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGame(int id, GameCreateUpdateDto gameDto, IFormFile imageFile)
        {
            Console.WriteLine($"Received form data - GameId: {gameDto.GameId}, ImagePath: {gameDto.ImagePath}, Title: {gameDto.Title}, Genre: {gameDto.Genre}, Price: {gameDto.Price}, ReleaseDate: {gameDto.ReleaseDate}, ImageFile: {(imageFile != null ? imageFile.FileName : "null")}");

            
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

                
                var existingGameForValidation = await _gameService.GetGameByIdAsync(id);
                if (existingGameForValidation != null)
                {
                    gameDto.ImagePath = existingGameForValidation.ImagePath;
                    Console.WriteLine($"Preserved ImagePath on validation failure: {gameDto.ImagePath}");
                }

                return View(gameDto);
            }
            if (id != gameDto.GameId)
                return BadRequest();

            var existingGame = await _gameService.GetGameByIdAsync(id);
            if (existingGame == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                gameDto.ImagePath = existingGame.ImagePath;
                return View(gameDto);
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                
                if (!string.IsNullOrEmpty(existingGame.ImagePath))
                {
                    string oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, existingGame.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                        System.IO.File.Delete(oldFilePath);
                }

                // Save new image
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/games");
                Directory.CreateDirectory(uploadsFolder);
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                gameDto.ImagePath = "/images/games/" + uniqueFileName;
            }
            else
            {
                // Preserve old image path from hidden field
                gameDto.ImagePath = Request.Form["ExistingImagePath"];
            }

            var updateResult = await _gameService.UpdateGameAsync(id, gameDto);
            if (!updateResult)
            {
                Console.WriteLine($"UpdateGameAsync failed for GameId: {id}");
                
                gameDto.ImagePath = existingGame.ImagePath;
                return View(gameDto);
            }

            return RedirectToAction("Index", "ManageCatalog");
        }

        /// <summary>
        /// Handles the HTTP POST request to permanently delete a specific game from the catalog.
        /// </summary>
        /// <param name="id">The unique identifier of the game to be deleted.</param>
        /// <returns>
        /// Catalog management page on successful deletion.
        /// Returns (HTTP 400) if the deletion operation fails.
        /// </returns>
        /// <example>
        /// POST: /GameController/Delete/5
        /// </example>

        // DELETE GAME - POST (for modal)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _gameService.DeleteGameAsync(id);
            if (!success)
            {
                return BadRequest();
            }
            return RedirectToAction("Index", "ManageCatalog");
        }

    }
}
