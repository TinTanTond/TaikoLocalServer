using Application.Handlers.Api.User;
using Shared.Models.Requests;
using Shared.Models.Responses;

namespace Server.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class UsersController : BaseController<UsersController>
{
    [HttpGet("my")]
    [Authorize(Policy = "AuthConditional")]
    public async Task<IActionResult> GetUser()
    {
        var baid = uint.Parse(User.Claims.First(c => c.Type == ClaimTypes.Name).Value);
        var query = new GetUserQuery(baid);
        var apiResult = await Mediator.Send(query);
        
        if (!apiResult.Succeeded)
        {
            return BadRequest(apiResult.Message);
        }
        
        return Ok(apiResult.Data);
    }
    
    [HttpGet("{baid}")]
    [Authorize(Policy = "AuthConditionalAdmin")]
    public async Task<User?> GetUser(uint baid)
    {
        var query = new GetUserQuery(baid);
        var apiResult = await Mediator.Send(query);
        
        if (!apiResult.Succeeded)
        {
            return null;
        }
        
        return apiResult.Data;
    }
    
    [HttpGet]
    [Authorize(Policy = "AuthConditionalAdmin")]
    public async Task<ActionResult<UsersResponse>> GetUsers([FromQuery] GetUsersRequest request)
    {
        var query = new GetUsersQuery(request.Page, request.Limit, request.SearchTerm);
        var apiResult = await Mediator.Send(query);
        
        if (!apiResult.Succeeded)
        {
            return BadRequest(apiResult.Message);
        }
        
        return Ok(apiResult.Data);
    }
    
    [HttpDelete("{baid}")]
    [Authorize(Policy = "AuthConditionalAdmin")]
    public async Task<IActionResult> DeleteUser(uint baid)
    {
        var command = new DeleteUserCommand(baid);
        var apiResult = await Mediator.Send(command);
        if (!apiResult.Succeeded)
        {
            return BadRequest(apiResult.Message);
        }
        
        return NoContent();
    }
    
    [HttpDelete("me")]
    [Authorize(Policy = "AuthConditional")]
    public async Task<IActionResult> DeleteCurrentUser()
    {
        var baid = uint.Parse(User.Claims.First(c => c.Type == ClaimTypes.Name).Value);
        var command = new DeleteUserCommand(baid);
        var apiResult = await Mediator.Send(command);
        if (!apiResult.Succeeded)
        {
            return BadRequest(apiResult.Message);
        }
        
        return NoContent();
    }
}