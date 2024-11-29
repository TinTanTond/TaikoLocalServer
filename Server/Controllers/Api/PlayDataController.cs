using Application.Handlers.Api.User;
using Shared.Models.Responses;

namespace Server.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class PlayDataController : BaseController<PlayDataController>
{
    [HttpGet("{baid}")]
    [Authorize(Policy = "AuthConditionalAdmin")]
    public async Task<IActionResult> GetSongBestRecords(uint baid)
    {
        var query = new GetSongBestRecordsQuery(baid);
        var apiResult = await Mediator.Send(query);
        if (!apiResult.Succeeded)
        {
            return BadRequest(apiResult.Message);
        }
        
        return Ok(apiResult.Data);
    }
    
    [HttpGet("my")]
    [Authorize(Policy = "AuthConditional")]
    public async Task<IActionResult> GetSongBestRecords()
    {
        var baid = uint.Parse(User.Claims.First(c => c.Type == ClaimTypes.Name).Value);
        var query = new GetSongBestRecordsQuery(baid);
        var apiResult = await Mediator.Send(query);
        if (!apiResult.Succeeded)
        {
            return BadRequest(apiResult.Message);
        }
        
        return Ok(apiResult.Data);
    }
}