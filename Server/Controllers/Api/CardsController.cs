using System.Security.Claims;
using Application.Handlers.Api.Cards;
using Microsoft.AspNetCore.Authorization;
using Shared.Models.Requests;

namespace Server.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class CardsController : BaseController<CardsController>
{
    [HttpDelete("admin/{accessCode}")]
    [Authorize(Policy = "AuthConditionalAdmin")]
    public async Task<IActionResult> DeleteAccessCodeAdmin(string accessCode)
    {
        var command = new DeleteCardCommand(accessCode, 0, true);
        var apiResult = await Mediator.Send(command);
        if (!apiResult.Succeeded)
        {
            return BadRequest(apiResult.Message);
        }

        return NoContent();
    }
    
    [HttpDelete("{accessCode}")]
    [Authorize(Policy = "AuthConditional")]
    public async Task<IActionResult> DeleteAccessCode(string accessCode)
    {
        var baid = uint.Parse(User.Claims.First(c => c.Type == ClaimTypes.Name).Value);
        var command = new DeleteCardCommand(accessCode, baid, false);
        var apiResult = await Mediator.Send(command);
        if (!apiResult.Succeeded)
        {
            return BadRequest(apiResult.Message);
        }

        return NoContent();
    }

    [HttpPost]
    [Authorize(Policy = "AuthConditional")]
    public async Task<IActionResult> BindAccessCode(BindAccessCodeRequest request)
    {
        if (request.Baid != 0)
        {
            return BadRequest("Baid must be 0");
        }
        var baid = uint.Parse(User.Claims.First(c => c.Type == ClaimTypes.Name).Value);
        var command = new BindAccessCodeCommand(request.AccessCode, baid);
        var apiResult = await Mediator.Send(command);
        if (!apiResult.Succeeded)
        {
            return BadRequest(apiResult.Message);
        }

        return Ok();
    }
    
    [HttpPost]
    [Authorize(Policy = "AuthConditionalAdmin")]
    public async Task<IActionResult> BindAccessCodeAdmin(BindAccessCodeRequest request)
    {
        var command = new BindAccessCodeCommand(request.AccessCode, request.Baid);
        var apiResult = await Mediator.Send(command);
        if (!apiResult.Succeeded)
        {
            return BadRequest(apiResult.Message);
        }

        return Ok();
    }
}
