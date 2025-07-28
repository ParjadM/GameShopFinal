using GameShop.DTO;
using GameShop.Dtos;

namespace GameShop.Services
{
    public interface IGameService
    {
        Task<IEnumerable<GameDto>> GetAllGamesAsync();
        Task<GameDto?> GetGameByIdAsync(int id);
        Task<GameDto> CreateGameAsync(GameCreateUpdateDto gameDto);
        Task<bool> UpdateGameAsync(int id, GameCreateUpdateDto gameDto);
        Task<bool> DeleteGameAsync(int id);
    }
}
