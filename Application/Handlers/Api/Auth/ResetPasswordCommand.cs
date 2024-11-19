namespace Application.Handlers.Api.Auth;

public record ResetPasswordCommand(uint Baid) : IRequest<ApiResult<bool>>;

// ResetPasswordCommandHandler.cs
public class ResetPasswordCommandHandler(ITaikoDbContext context, ILogger<ResetPasswordCommandHandler> logger)
    : IRequestHandler<ResetPasswordCommand, ApiResult<bool>>
{
    public async Task<ApiResult<bool>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var credential = await context.Credentials.FirstOrDefaultAsync(c => c.Baid == request.Baid, cancellationToken);
        if (credential is null)
        {
            return ApiResult.Failed<bool>("Credential not found");
        }

        credential.Password = string.Empty;
        credential.Salt = string.Empty;

        await context.SaveChangesAsync(cancellationToken);
        return ApiResult.Succeed(true);
    }
} 