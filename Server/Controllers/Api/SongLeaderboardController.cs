using System.Security.Claims;
using Application.Handlers.Api.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
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
}