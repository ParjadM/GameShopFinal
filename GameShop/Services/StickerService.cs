using GameShop.Data;
using GameShop.DTO;
using GameShop.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameShop.Services
{
    public class StickerService : IStickerService
    {
        private readonly ApplicationDbContext _context;

        public StickerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<StickerDto> CreateAsync(StickerCreateUpdateDto dto)
        {
            var sticker = new Sticker
            {
                GameId = dto.GameId,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                ImageUrl = dto.ImageUrl
            };

            _context.Stickers.Add(sticker);
            await _context.SaveChangesAsync();

            return new StickerDto
            {
                StickerId = sticker.StickerId,
                Name = sticker.Name,
                Description = sticker.Description,
                Price = sticker.Price,
                ImageUrl = sticker.ImageUrl,
                GameTitle = (await _context.Games.FindAsync(sticker.GameId))?.Title
            };
        }

        public async Task<IEnumerable<StickerDto>> GetAllAsync()
        {
            return await _context.Stickers
                .Include(s => s.Game)
                .Select(s => new StickerDto
                {
                    StickerId = s.StickerId,
                    Name = s.Name,
                    Description = s.Description,
                    Price = s.Price,
                    ImageUrl = s.ImageUrl,
                    GameTitle = s.Game != null ? s.Game.Title : null
                }).ToListAsync();
        }

        public async Task<StickerDto?> GetByIdAsync(int stickerId)
        {
            var s = await _context.Stickers
                .Include(sticker => sticker.Game)
                .FirstOrDefaultAsync(sticker => sticker.StickerId == stickerId);

            if (s == null) return null;

            return new StickerDto
            {
                StickerId = s.StickerId,
                Name = s.Name,
                Description = s.Description,
                Price = s.Price,
                ImageUrl = s.ImageUrl,
                GameTitle = s.Game?.Title
            };
        }

        public async Task<IEnumerable<StickerDto>> GetByGameIdAsync(int gameId)
        {
            return await _context.Stickers
                .Where(s => s.GameId == gameId)
                .Include(s => s.Game)
                .Select(s => new StickerDto
                {
                    StickerId = s.StickerId,
                    Name = s.Name,
                    Description = s.Description,
                    Price = s.Price,
                    ImageUrl = s.ImageUrl,
                    GameTitle = s.Game != null ? s.Game.Title : null
                }).ToListAsync();
        }

        public async Task<bool> UpdateAsync(int stickerId, StickerCreateUpdateDto dto)
        {
            var sticker = await _context.Stickers.FindAsync(stickerId);
            if (sticker == null) return false;

            sticker.GameId = dto.GameId;
            sticker.Name = dto.Name;
            sticker.Description = dto.Description;
            sticker.Price = dto.Price;
            sticker.ImageUrl = dto.ImageUrl;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int stickerId)
        {
            var sticker = await _context.Stickers.FindAsync(stickerId);
            if (sticker == null) return false;

            _context.Stickers.Remove(sticker);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
