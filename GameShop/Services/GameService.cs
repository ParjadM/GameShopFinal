using GameShop.Data;
using GameShop.DTO;
using GameShop.Dtos;
using GameShop.Models;
using Microsoft.EntityFrameworkCore;

namespace GameShop.Services
{
    // Constructor - set up database context
    public class GameService : IGameService
    {
        private readonly ApplicationDbContext _context;

        public GameService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create a new game
        public async Task<GameDto> CreateGameAsync(GameCreateUpdateDto gameDto)
        {
            var game = new Game
            {
                Title = gameDto.Title,
                Genre = gameDto.Genre,
                Price = gameDto.Price,
                ReleaseDate = gameDto.ReleaseDate,
                ImagePath = gameDto.ImagePath
            };

            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            return new GameDto
            {
                GameId = game.GameId,
                Title = game.Title,
                Genre = game.Genre,
                ImagePath = game.ImagePath
            };
        }

        // Delete a game by ID
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

        // Get all games with stickers
        public async Task<IEnumerable<GameDto>> GetAllGamesAsync()
        {
            return await _context.Games
                .Include(g => g.Stickers)
                .Select(g => new GameDto
                {
                    GameId = g.GameId,
                    Title = g.Title,
                    Genre = g.Genre,
                    ImagePath = g.ImagePath, // image path
                    Stickers = g.Stickers.Select(s => new StickerDto
                    {
                        StickerId = s.StickerId,
                        Name = s.Name,

                    }).ToList()
                })
                .ToListAsync();
        }

        // Get a single game by ID
        public async Task<GameDto?> GetGameByIdAsync(int id)
        {
            var game = await _context.Games
                .Where(g => g.GameId == id)
                .Select(g => new GameDto
                {
                    GameId = g.GameId,
                    Title = g.Title,
                    Genre = g.Genre,
                    Price = g.Price,
                    ReleaseDate = g.ReleaseDate,

                    ImagePath = g.ImagePath
                })
                .FirstOrDefaultAsync();

            return game;
        }

        // Update an existing game
        public async Task<bool> UpdateGameAsync(int id, GameCreateUpdateDto gameDto)
        {
            Console.WriteLine($"Updating game {id}, ImagePath: {gameDto.ImagePath}, Title: {gameDto.Title}, Genre: {gameDto.Genre}, Price: {gameDto.Price}, ReleaseDate: {gameDto.ReleaseDate}");

            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                Console.WriteLine($"Game not found for ID: {id}");
                return false;
            }

            game.Title = gameDto.Title;
            game.Genre = gameDto.Genre;
            game.Price = gameDto.Price;
            game.ReleaseDate = gameDto.ReleaseDate;
            game.ImagePath = gameDto.ImagePath;

            try
            {
                await _context.SaveChangesAsync();
                Console.WriteLine($"Game {id} updated successfully");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to update game {id}. Error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                return false;
            }
        }
    }
}
