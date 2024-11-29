using Application.Handlers.Api.User;
using Shared.Models.Requests;

namespace Server.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class FavoriteSongsController : BaseController<FavoriteSongsController>
{
    [HttpPost]
    [Authorize(Policy = "AuthConditional")]
    public async Task<IActionResult> UpdateFavoriteSong(SetFavoriteRequest request)
    {
        if (request.Baid != 0)
        {
            return BadRequest("Baid must be 0");
        }
        var baid = uint.Parse(User.Claims.First(c => c.Type == ClaimTypes.Name).Value);
        var command = new UpdateFavoriteSongCommand(baid, request.SongId, request.IsFavorite);
        var apiResult = await Mediator.Send(command);
        if (!apiResult.Succeeded)
        {
            return BadRequest(apiResult.Message);
        }
        
        return NoContent();
    }
    
    [HttpPost]
    [Authorize(Policy = "AuthConditionalAdmin")]
    public async Task<IActionResult> UpdateFavoriteSongAdmin(SetFavoriteRequest request)
    {
        var command = new UpdateFavoriteSongCommand(request.Baid, request.SongId, request.IsFavorite);
        var apiResult = await Mediator.Send(command);
        if (!apiResult.Succeeded)
        {
            return BadRequest(apiResult.Message);
        }
        
        return NoContent();
    }

    [HttpGet("my")]
    [Authorize(Policy = "AuthConditional")]
    public async Task<IActionResult> GetFavoriteSongs()
    {
        var baid = uint.Parse(User.Claims.First(c => c.Type == ClaimTypes.Name).Value);
        
        var query = new GetFavoriteSongsQuery(baid);
        var apiResult = await Mediator.Send(query);
        if (!apiResult.Succeeded)
        {
            return BadRequest(apiResult.Message);
        }
        
        return Ok(apiResult.Data);
    }
    
    [HttpGet("{baid}")]
    [Authorize(Policy = "AuthConditionalAdmin")]
    public async Task<IActionResult> GetFavoriteSongs(uint baid)
    {
        var query = new GetFavoriteSongsQuery(baid);
        var apiResult = await Mediator.Send(query);
        if (!apiResult.Succeeded)
        {
            return BadRequest(apiResult.Message);
        }
        
        return Ok(apiResult.Data);
    }
}