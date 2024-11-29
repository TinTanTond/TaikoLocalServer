using Application.Handlers.Api.User;
using Shared.Models.Requests;

namespace Server.Controllers.Api;

[ApiController]
[Route("api/[controller]")]

public class SongLeaderboardController : BaseController<SongLeaderboardController>
{
    [HttpGet("{songId}")]
    [Authorize("AuthConditional")]
    public async Task<IActionResult> GetSongLeaderboard([FromQuery]GetSongLeaderboardRequest request)
    {
        var baid = uint.Parse(User.Claims.First(c => c.Type == ClaimTypes.Name).Value);
        var query = new GetSongLeaderboardQuery(request.SongId, request.Difficulty, baid, request.Page, request.Limit);
        var apiResult = await Mediator.Send(query);
        if (!apiResult.Succeeded)
        {
            return BadRequest(apiResult.Message);
        }

        return Ok(apiResult.Data);
    }
    
    [HttpGet("admin/{songId}")]
    [Authorize(Policy = "AuthConditionalAdmin")]
    public async Task<IActionResult> GetSongLeaderboardAdmin([FromQuery]GetSongLeaderboardRequest request)
    {
        var query = new GetSongLeaderboardQuery(request.SongId, request.Difficulty, request.Baid, request.Page, request.Limit);
        var apiResult = await Mediator.Send(query);
        if (!apiResult.Succeeded)
        {
            return BadRequest(apiResult.Message);
        }

        return Ok(apiResult.Data);
    }
}