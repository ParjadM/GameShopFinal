using GameShop.DTO;
using GameShop.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StickersController : ControllerBase
    {
        private readonly IStickerService _stickerService;

        public StickersController(IStickerService stickerService)
        {
            _stickerService = stickerService;
        }

        // GET: api/stickers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StickerDto>>> GetAll()
        {
            var stickers = await _stickerService.GetAllAsync();
            return Ok(stickers);
        }

        // GET: api/stickers/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<StickerDto>> GetById(int id)
        {
            var sticker = await _stickerService.GetByIdAsync(id);
            if (sticker == null) return NotFound();
            return Ok(sticker);
        }

        // GET: api/stickers/game/{gameId}
        [HttpGet("game/{gameId:int}")]
        public async Task<ActionResult<IEnumerable<StickerDto>>> GetByGameId(int gameId)
        {
            var stickers = await _stickerService.GetByGameIdAsync(gameId);
            return Ok(stickers);
        }

        // POST: api/stickers
        [HttpPost]
        public async Task<ActionResult<StickerDto>> Create(StickerCreateUpdateDto dto)
        {
            var created = await _stickerService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.StickerId }, created);
        }

        // PUT: api/stickers/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, StickerCreateUpdateDto dto)
        {
            var updated = await _stickerService.UpdateAsync(id, dto);
            if (!updated) return NotFound();
            return NoContent();
        }

        // DELETE: api/stickers/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _stickerService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
