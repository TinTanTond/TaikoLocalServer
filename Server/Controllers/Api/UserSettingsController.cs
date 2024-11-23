using System.Security.Claims;
using Application.Handlers.Api.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Server.Controllers.Api;

[ApiController]
[Route("/api/[controller]")]
public class UserSettingsController : BaseController<UserSettingsController>
{
    [HttpPost("{baid}")]
    [Authorize(Policy = "AuthConditionalAdmin")]
    public async Task<IActionResult> SaveUserSetting(uint baid, UserSetting userSetting)
    {
        var command = new UpdateUserSettingsCommand(baid, userSetting);
        var result = await Mediator.Send(command);
        if (!result.Succeeded)
        {
            return BadRequest(result.Message);
        }

        return NoContent();
    }
    
    [HttpPost("my")]
    [Authorize(Policy = "AuthConditional")]
    public async Task<IActionResult> SaveMyUserSetting(UserSetting userSetting)
    {
        var baid = uint.Parse(User.Claims.First(c => c.Type == ClaimTypes.Name).Value);
        var command = new UpdateUserSettingsCommand(baid, userSetting);
        var result = await Mediator.Send(command);
        if (!result.Succeeded)
        {
            return BadRequest(result.Message);
        }

        return NoContent();
    }

}