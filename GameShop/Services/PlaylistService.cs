// In file: GameShop/Services/PlaylistService.cs
using GameShop.Data;
using GameShop.DTO;
using GameShop.Services;
using Microsoft.EntityFrameworkCore;
using GameShop.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// The namespace should match your project structure
namespace GameShop.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly ApplicationDbContext _context;

        public PlaylistService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PlaylistDto> CreatePlaylistAsync(PlaylistCreateUpdateDto playlistDto)
        {
            var playlist = new Playlist
            {
                Name = playlistDto.Name,
                Description = playlistDto.Description,
                DateCreated = System.DateTime.UtcNow 
            };

            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();

            return new PlaylistDto
            {
                PlaylistId = playlist.PlaylistID, 
                Name = playlist.Name,
                Description = playlist.Description
            };
        }

        public async Task<PlaylistDto?> GetPlaylistByIdAsync(int playlistId)
        {
            var playlist = await _context.Playlists
                .Include(p => p.PlaylistGames)
                .ThenInclude(pg => pg.Game)
                .Where(p => p.PlaylistID == playlistId)
                .Select(p => new PlaylistDto
                {
                    PlaylistId = p.PlaylistID, 
                    Name = p.Name,
                    Description = p.Description,
                    Games = p.PlaylistGames.Select(pg => new GameDto
                    {
                        GameId = pg.Game.GameId,
                        Title = pg.Game.Title,
                        Genre = pg.Game.Genre,
                        Price = pg.Game.Price, 
                        ReleaseDate = pg.Game.ReleaseDate, 
                        ImageUrl = pg.Game.ImagePath 
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return playlist;
        }

        public async Task<IEnumerable<PlaylistDto>> GetAllPlaylistsAsync()
        {
            var playlists = await _context.Playlists
                .Select(p => new PlaylistDto
                {
                    PlaylistId = p.PlaylistID, 
                    Name = p.Name,
                    Description = p.Description,
                    Games = new List<GameDto>() 
                })
                .ToListAsync();

            return playlists;
        }

        public async Task<bool> UpdatePlaylistAsync(int playlistId, PlaylistCreateUpdateDto playlistDto)
        {
            var playlistToUpdate = await _context.Playlists.FindAsync(playlistId);

            if (playlistToUpdate == null)
            {
                return false;
            }

            playlistToUpdate.Name = playlistDto.Name;
            playlistToUpdate.Description = playlistDto.Description;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeletePlaylistAsync(int playlistId)
        {
            var playlist = await _context.Playlists.FindAsync(playlistId);
            if (playlist == null)
            {
                return false;
            }

            _context.Playlists.Remove(playlist);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddGameToPlaylistAsync(int playlistId, int gameId)
        {
            var playlistExists = await _context.Playlists.AnyAsync(p => p.PlaylistID == playlistId);
            var gameExists = await _context.Games.AnyAsync(g => g.GameId == gameId);

            if (!playlistExists || !gameExists)
            {
                return false;
            }

            var associationExists = await _context.PlaylistGames
                .AnyAsync(pg => pg.PlaylistId == playlistId && pg.GameId == gameId);

            if (associationExists)
            {
                return true; // Already exists, so the operation is successful
            }

            var playlistGame = new PlaylistGame
            {
                PlaylistId = playlistId,
                GameId = gameId
            };

            _context.PlaylistGames.Add(playlistGame);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveGameFromPlaylistAsync(int playlistId, int gameId)
        {
            var playlistGame = await _context.PlaylistGames
                .FirstOrDefaultAsync(pg => pg.PlaylistId == playlistId && pg.GameId == gameId);

            if (playlistGame == null)
            {
                return false;
            }

            _context.PlaylistGames.Remove(playlistGame);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}