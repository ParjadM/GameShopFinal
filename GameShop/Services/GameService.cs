using GameShop.Data;
using GameShop.DTO;
using GameShop.Dtos;
using GameShop.Models;
using Microsoft.EntityFrameworkCore;

namespace GameShop.Services
{
    public class GameService : IGameService
    {
        private readonly ApplicationDbContext _context;

        public GameService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GameDto> CreateGameAsync(GameCreateUpdateDto gameDto)
        {
            var game = new Game
            {
                Title = gameDto.Title,
                Genre = gameDto.Genre,
                Price = gameDto.Price,
                ReleaseDate = gameDto.ReleaseDate
            };

            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            return new GameDto 
            {
                GameId = game.GameId,
                Title = game.Title,
                Genre = game.Genre
            };
        }

        public async Task<bool> DeleteGameAsync(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return false;
            }

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<GameDto>> GetAllGamesAsync()
        {
            return await _context.Games
                .Select(g => new GameDto
                {
                    GameId = g.GameId,
                    Title = g.Title,
                    Genre = g.Genre
                })
                .ToListAsync();
        }

        public async Task<GameDto?> GetGameByIdAsync(int id)
        {
            var game = await _context.Games
                .Where(g => g.GameId == id)
                .Select(g => new GameDto
                {
                    GameId = g.GameId,
                    Title = g.Title,
                    Genre = g.Genre
                })
                .FirstOrDefaultAsync();

            return game;
        }

        public async Task<bool> UpdateGameAsync(int id, GameCreateUpdateDto gameDto)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return false;
            }

            game.Title = gameDto.Title;
            game.Genre = gameDto.Genre;
            game.Price = gameDto.Price;
            game.ReleaseDate = gameDto.ReleaseDate;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
