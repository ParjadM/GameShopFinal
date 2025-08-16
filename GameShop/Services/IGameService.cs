using GameShop.DTO;
using GameShop.Dtos;

namespace GameShop.Services
{
    // Interface for game-related operations
    public interface IGameService
    {
        // Get all games
        Task<IEnumerable<GameDto>> GetAllGamesAsync();
        // Get a game by ID
        Task<GameDto?> GetGameByIdAsync(int id);
        // Create a new game
        Task<GameDto> CreateGameAsync(GameCreateUpdateDto gameDto);
        // Update an existing game
        Task<bool> UpdateGameAsync(int id, GameCreateUpdateDto gameDto);
        // Delete a game by ID
        Task<bool> DeleteGameAsync(int id);
    }
}
