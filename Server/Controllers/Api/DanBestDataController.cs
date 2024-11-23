using System.Security.Claims;
using Application.Handlers.Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Server.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class DanBestDataController : BaseController<DanBestDataController>
{
    [HttpGet("my")]
    [Authorize(Policy = "AuthConditional")]
    public async Task<IActionResult> GetDanBestData()
    {
        var baid = uint.Parse(User.Claims.First(c => c.Type == ClaimTypes.Name).Value);
        
        var query = new GetDanBestDataQuery(baid);
        var apiResult = await Mediator.Send(query);
        if (!apiResult.Succeeded)
        {
            return BadRequest(apiResult.Message);
        }
        
        return Ok(apiResult.Data);
    }
    
    [HttpGet("{baid}")]
    [Authorize(Policy = "AuthConditionalAdmin")]
    public async Task<IActionResult> GetDanBestData(uint baid)
    {
        var query = new GetDanBestDataQuery(baid);
        var apiResult = await Mediator.Send(query);
        if (!apiResult.Succeeded)
        {
            return BadRequest(apiResult.Message);
        }
        
        return Ok(apiResult.Data);
    }
}