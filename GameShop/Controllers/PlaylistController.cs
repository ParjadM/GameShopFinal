using GameShop.DTO;
using GameShop.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GameShop.Controllers
{
    public class PlaylistController : Controller
    {
        private readonly IPlaylistService _playlistService;

        public PlaylistController(IPlaylistService playlistService)
        {
            _playlistService = playlistService;
        }

        // GET: Show the AddPlaylist view
        public IActionResult Add()
        {
            return View("AddPlaylist");
        }

        // POST: Create playlist
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(PlaylistCreateUpdateDto playlistDto)
        {
            if (!ModelState.IsValid)
                return View("AddPlaylist", playlistDto);

            await _playlistService.CreatePlaylistAsync(playlistDto);
            return RedirectToAction("Index", "ManageCatalog");
        }

        // GET: Playlist/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var playlist = await _playlistService.GetPlaylistByIdAsync(id);
            if (playlist == null) return NotFound();

            var dto = new PlaylistCreateUpdateDto
            {
                Name = playlist.Name,
                Description = playlist.Description
            };

            return View("EditPlaylist", dto);
        }

        // POST: Playlist/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PlaylistCreateUpdateDto playlistDto)
        {
            if (!ModelState.IsValid)
                return View("EditPlaylist", playlistDto);

            var success = await _playlistService.UpdatePlaylistAsync(id, playlistDto);
            if (!success) return NotFound();

            return RedirectToAction("Index", "ManageCatalog");
        }

        // POST: Playlist/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _playlistService.DeletePlaylistAsync(id);
            if (!success) return NotFound();

            return RedirectToAction("Index", "ManageCatalog");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddGame([FromBody] PlaylistGameDto dto)
        {
            var result = await _playlistService.AddGameToPlaylistAsync(dto.PlaylistId, dto.GameId);
            if (result) return Ok();
            return BadRequest();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveGame([FromBody] PlaylistGameDto dto)
        {
            var result = await _playlistService.RemoveGameFromPlaylistAsync(dto.PlaylistId, dto.GameId);
            if (result) return Ok();
            return BadRequest();
        }

        // DTO for AJAX calls
        public class PlaylistGameDto
        {
            public int PlaylistId { get; set; }
            public int GameId { get; set; }
        }


    }
}
