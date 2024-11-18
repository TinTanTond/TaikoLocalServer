using Application.Models.Api;

namespace Application.Handlers.Api;

public record RegisterCommand : IRequest<ApiResult<bool>>
{
    public string AccessCode { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool RegisterWithLastPlayTime { get; set; }
    public DateTime LastPlayDateTime { get; set; }
    public string InviteCode { get; set; } = string.Empty;
}

public class RegisterCommandHandler(ITaikoDbContext context, ILogger<RegisterCommandHandler> logger)
    : IRequestHandler<RegisterCommand, ApiResult<bool>>
{
    public async Task<ApiResult<bool>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var card = await context.Cards.Include(card => card.Ba)
            .ThenInclude(user => user!.Credential)
            .FirstOrDefaultAsync(card => card.AccessCode == request.AccessCode, cancellationToken);
        if (card is null)
        {
            return ApiResult.Failed<bool>("Invalid access code");
        }

        var user = card.Ba;
        if (user!.Credential is not null && user.Credential.Password != string.Empty)
        {
            return ApiResult.Failed<bool>("User already registered");
        }

        // TODO: Fix this using server side config
        if (request.RegisterWithLastPlayTime)
        {
            var invited = TotpUtils.VerifyOtp(request.InviteCode, card.Baid);

            if (!invited)
            {
                var diffMinutes = (request.LastPlayDateTime - user.LastPlayDatetime).Duration().TotalMinutes;
                if (diffMinutes > 5)
                {
                    return ApiResult.Failed<bool>("Wrong Last Play Time");
                }
            }
        }

        var salt = BCrypt.Net.BCrypt.GenerateSalt();
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password, salt);

        if (user.Credential is null)
        {
            user.Credential = new Credential
            {
                Baid = user.Baid,
                Password = hashedPassword,
                Salt = ""
            };
        }
        else
        {
            user.Credential.Password = hashedPassword;
        }

        await context.SaveChangesAsync(cancellationToken);
        return ApiResult.Succeed(true);
    }
}
