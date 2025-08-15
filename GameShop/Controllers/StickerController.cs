using GameShop.DTO;
using GameShop.Dtos;
using GameShop.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameShop.Controllers
{
    public class StickerController : Controller
    {
        private readonly IStickerService _stickerService;
        private readonly IGameService _gameService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<StickerController> _logger;

        public StickerController(
            IStickerService stickerService,
            IGameService gameService,
            IWebHostEnvironment webHostEnvironment,
            ILogger<StickerController> logger)
        {
            _stickerService = stickerService;
            _gameService = gameService;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        // GET: Sticker/Add
        [HttpGet]
        public async Task<IActionResult> Add(int? gameId)
        {
            _logger.LogInformation("GET Add Sticker called. Pre-selected GameId: {GameId}", gameId);

            var games = await _gameService.GetAllGamesAsync();
            ViewBag.Games = games.Select(g => new SelectListItem
            {
                Value = g.GameId.ToString(),
                Text = g.Title,
                Selected = gameId.HasValue && g.GameId == gameId.Value
            }).ToList();

            var model = new StickerCreateUpdateDto();
            if (gameId.HasValue)
                model.GameId = gameId.Value;

            return View("AddSticker", model);
        }

        // POST: Sticker/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(StickerCreateUpdateDto dto, IFormFile imageFile)
        {
            _logger.LogInformation("POST Add Sticker called. Received DTO: {@Dto}", dto);
            _logger.LogInformation("Selected GameId: {GameId}", dto.GameId);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid. Errors: {Errors}",
                    string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));

                var games = await _gameService.GetAllGamesAsync();
                ViewBag.Games = games.Select(g => new SelectListItem
                {
                    Value = g.GameId.ToString(),
                    Text = g.Title,
                    Selected = g.GameId == dto.GameId
                }).ToList();

                return View("AddSticker", dto);
            }

            // Handle image
            if (imageFile != null && imageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/stickers");
                Directory.CreateDirectory(uploadsFolder);
                string uniqueFileName = Guid.NewGuid() + "_" + Path.GetFileName(imageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                _logger.LogInformation("Saving uploaded image to: {FilePath}", filePath);

                using var fs = new FileStream(filePath, FileMode.Create);
                await imageFile.CopyToAsync(fs);

                dto.ImageUrl = "/images/stickers/" + uniqueFileName;
            }

            await _stickerService.CreateAsync(dto);
            _logger.LogInformation("Sticker successfully created. Redirecting to ManageCatalog Index.");

            return RedirectToAction("Index", "ManageCatalog");
        }


        // GET: Sticker/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var sticker = await _stickerService.GetByIdAsync(id);
            if (sticker == null)
                return NotFound();

            var model = new StickerCreateUpdateDto
            {
                Name = sticker.Name,
                Description = sticker.Description,
                Price = sticker.Price,
                GameId = sticker.GameId,
                ImageUrl = sticker.ImageUrl
            };

            // Populate dropdown
            var games = await _gameService.GetAllGamesAsync();
            ViewBag.Games = games.Select(g => new SelectListItem
            {
                Value = g.GameId.ToString(),
                Text = g.Title,
                Selected = g.GameId == model.GameId
            }).ToList();

            return View("EditSticker", model);
        }

        // POST: Sticker/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StickerCreateUpdateDto dto, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
            {
                var games = await _gameService.GetAllGamesAsync();
                ViewBag.Games = games.Select(g => new SelectListItem
                {
                    Value = g.GameId.ToString(),
                    Text = g.Title,
                    Selected = g.GameId == dto.GameId
                }).ToList();

                return View("EditSticker", dto);
            }

            // Handle new image upload if provided
            if (imageFile != null && imageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/stickers");
                Directory.CreateDirectory(uploadsFolder);
                string uniqueFileName = Guid.NewGuid() + "_" + imageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using var fs = new FileStream(filePath, FileMode.Create);
                await imageFile.CopyToAsync(fs);

                dto.ImageUrl = "/images/stickers/" + uniqueFileName;
            }

            bool updated = await _stickerService.UpdateAsync(id, dto);
            if (!updated)
                return NotFound();

            return RedirectToAction("Index", "ManageCatalog");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            // Call the service to delete the sticker
            bool deleted = await _stickerService.DeleteAsync(id);

            if (!deleted)
            {
                // Optional: show an error message if deletion failed
                TempData["Error"] = "Sticker not found or could not be deleted.";
            }

            // Redirect back to ManageCatalog index
            return RedirectToAction("Index", "ManageCatalog");
        }

    }
}
