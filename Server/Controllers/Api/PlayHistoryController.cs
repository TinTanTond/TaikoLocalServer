using System.Security.Claims;
using Application.Handlers.Api.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Server.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class PlayHistoryController : BaseController<PlayDataController>
{
    [HttpGet("{baid}")]
    [Authorize(Policy = "AuthConditionalAdmin")]
    public async Task<IActionResult> GetSongHistory(uint baid)
    {
        var query = new GetSongHistoryQuery(baid);
        var apiResult = await Mediator.Send(query);
        if (!apiResult.Succeeded)
        {
            return BadRequest(apiResult.Message);
        }
        
        return Ok(apiResult.Data);
    }
    
    [HttpGet("my")]
    [Authorize(Policy = "AuthConditional")]
    public async Task<IActionResult> GetSongHistory()
    {
        var baid = uint.Parse(User.Claims.First(c => c.Type == ClaimTypes.Name).Value);
        var query = new GetSongHistoryQuery(baid);
        var apiResult = await Mediator.Send(query);
        if (!apiResult.Succeeded)
        {
            return BadRequest(apiResult.Message);
        }
        
        return Ok(apiResult.Data);
    }
}