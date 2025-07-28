// Controllers/PlaylistsController.cs
using GameShop.DTO;
using GameShop.Services;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class PlaylistsController : ControllerBase
{
    private readonly IPlaylistService _playlistService;

    public PlaylistsController(IPlaylistService playlistService)
    {
        _playlistService = playlistService;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePlaylist([FromBody] PlaylistCreateUpdateDto playlistDto)
    {
        var newPlaylist = await _playlistService.CreatePlaylistAsync(playlistDto);
        return CreatedAtAction(nameof(GetPlaylistById), new { id = newPlaylist.PlayListId }, newPlaylist);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPlaylistById(int id)
    {
        var playlist = await _playlistService.GetPlaylistByIdAsync(id);
        
        return Ok(playlist);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePlaylist(int id)
    {
        var success = await _playlistService.DeletePlaylistAsync(id);
        
        return NoContent(); 
    }
    [HttpPost("{playlistId}/games/{gameId}")]
    public async Task<IActionResult> AddGameToPlaylist(int playlistId, int gameId)
    {
        var success = await _playlistService.AddGameToPlaylistAsync(playlistId, gameId);
        
        return Ok();
    }

    [HttpDelete("{playlistId}/games/{gameId}")]
    public async Task<IActionResult> RemoveGameFromPlaylist(int playlistId, int gameId)
    {
        var success = await _playlistService.RemoveGameFromPlaylistAsync(playlistId, gameId);
        
        return NoContent();
    }
}