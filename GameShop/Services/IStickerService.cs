using GameShop.DTO;

namespace GameShop.Services
{
    public interface IStickerService
    {
        // CRUD for Stickers
        Task<StickerDto> CreateAsync(StickerCreateUpdateDto dto);
        Task<IEnumerable<StickerDto>> GetAllAsync();
        Task<StickerDto?> GetByIdAsync(int id);
        Task<IEnumerable<StickerDto>> GetByGameIdAsync(int gameId);
        Task<bool> UpdateAsync(int id, StickerCreateUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
