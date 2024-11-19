namespace Application.Handlers.Api.Auth;

public record LoginCommand(string AccessCode, string Password): IRequest<ApiResult<string>>;

public class LoginCommandHandler(ITaikoDbContext context, IJwtTokenService jwtTokenService,
    ILogger<LoginCommandHandler> logger) 
    : IRequestHandler<LoginCommand, ApiResult<string>>
{
    public async Task<ApiResult<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var card = await context.Cards.Include(card => card.Ba)
            .ThenInclude(user => user!.Credential)
            .FirstOrDefaultAsync(card => card.AccessCode == request.AccessCode, cancellationToken);
        if (card is null)
        {
            return ApiResult.Failed<string>("Invalid access code");
        }

        card.Ba.ThrowIfNull("Should not happen");
        var credential = card.Ba.Credential;
        if (credential is null || credential.Password == string.Empty)
        {
            return ApiResult.Failed<string>("User not registered");
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, credential.Password))
        {
            return ApiResult.Failed<string>("Invalid password");
        }

        var token = jwtTokenService.GenerateToken(card.Ba.Baid, card.Ba.IsAdmin);
        return ApiResult.Succeed(token);
    }
}