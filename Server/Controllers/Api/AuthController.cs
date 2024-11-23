using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Handlers.Api.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OtpNet;
using Shared.Models.Requests;

namespace Server.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class AuthController : BaseController<AuthController>
{
    
    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        var loginCommand = new LoginCommand(loginRequest.AccessCode, loginRequest.Password);
        var apiResult = await Mediator.Send(loginCommand);

        if (!apiResult.Succeeded)
        {
            return Unauthorized(apiResult.Message);
        }

        return Ok(apiResult.Data);
    }
    
    /*[HttpPost("LoginWithToken")]
    [ServiceFilter(typeof(AuthorizeIfRequiredAttribute))]
    public IActionResult LoginWithToken()
    {
        var tokenInfo = authService.ExtractTokenInfo(Microsoft.AspNetCore.Http.HttpContext);
        if (tokenInfo == null)
        {
            return Unauthorized();
        }

        return Ok();
    }*/
    

    [HttpPost("Register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterRequest registerRequest)
    {
        var registerCommand = RegisterCommandMapper.ToCommand(registerRequest);
        var apiResult = await Mediator.Send(registerCommand);

        if (!apiResult.Succeeded)
        {
            return BadRequest(apiResult.Message);
        }

        return Ok();
    }
    
    [HttpPost]
    [Authorize(Policy = "AuthConditional")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
    {
        var baid = uint.Parse(User.Claims.First(c => c.Type == ClaimTypes.Name).Value);
        var changePasswordCommand = new ChangePasswordCommand(baid, request.OldPassword, request.NewPassword);
        var apiResult = await Mediator.Send(changePasswordCommand);

        if (!apiResult.Succeeded)
        {
            return BadRequest(apiResult.Message);
        }
        
        return Ok();
    }

    [HttpPost]
    [Authorize(Policy = "AuthConditional")]
    public async Task<IActionResult> ChangePasswordAdmin(ChangePasswordRequest request)
    {
        var changePasswordCommand = ChangePasswordCommandMapper.ToCommand(request);
        var apiResult = await Mediator.Send(changePasswordCommand);
        
        if (!apiResult.Succeeded)
        {
            return BadRequest(apiResult.Message);
        }

        return Ok();
    }
    
    [HttpPost]
    [Authorize(Policy = "AuthConditionalAdmin")]
    public async Task<IActionResult> ResetPasswordAdmin(ResetPasswordRequest resetPasswordRequest)
    {
        var resetPasswordCommand = new ResetPasswordCommand(resetPasswordRequest.Baid);
        var apiResult = await Mediator.Send(resetPasswordCommand);

        if (!apiResult.Succeeded)
        {
            return BadRequest(apiResult.Message);
        }

        return Ok();
    }
    
    [HttpPost]
    [Authorize(Policy = "AuthConditional")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest resetPasswordRequest)
    {
        var baid = uint.Parse(User.Claims.First(c => c.Type == ClaimTypes.Name).Value);
        var resetPasswordCommand = new ResetPasswordCommand(baid);
        var apiResult = await Mediator.Send(resetPasswordCommand);

        if (!apiResult.Succeeded)
        {
            return Unauthorized(apiResult.Message);
        }

        return Ok();
    }

    [HttpPost("GenerateOtp")]
    [Authorize(Policy = "AuthConditionalAdmin")]    
    public IActionResult GenerateOtp(GenerateOtpRequest request)
    {
        var command = new GenerateOtpCommand(request.Baid);
        var apiResult = Mediator.Send(command);
        return Ok(apiResult.Result.Data);
    }
}
