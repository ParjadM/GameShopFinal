using GameShop.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GameShop.Services
{
    public interface IPlaylistService
    {
        // CRUD for Playlists
        Task<IEnumerable<PlaylistDto>> GetAllPlaylistsAsync();
        Task<PlaylistDto?> GetPlaylistByIdAsync(int playlistId);
        Task<PlaylistDto> CreatePlaylistAsync(PlaylistCreateUpdateDto playlistDto);
        Task<bool> UpdatePlaylistAsync(int playlistId, PlaylistCreateUpdateDto playlistDto);
        Task<bool> DeletePlaylistAsync(int playlistId);


        // Operations for the bridge table
        Task<bool> AddGameToPlaylistAsync(int playlistId, int gameId);
        Task<bool> RemoveGameFromPlaylistAsync(int playlistId, int gameId);

        Task<IEnumerable<PlaylistDto>> GetAllPlaylistsWithGamesAsync();
    }
}
