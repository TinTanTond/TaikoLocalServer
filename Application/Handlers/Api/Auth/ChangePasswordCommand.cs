namespace Application.Handlers.Api.Auth;

public record ChangePasswordCommand(uint Baid, string OldPassword, string NewPassword) : IRequest<ApiResult<bool>>;


public class ChangePasswordCommandHandler(ITaikoDbContext context, ILogger<ChangePasswordCommandHandler> logger)
    : IRequestHandler<ChangePasswordCommand, ApiResult<bool>>
{
    public async Task<ApiResult<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await context.UserData.Include(u => u.Credential)
            .FirstOrDefaultAsync(u => u.Baid == request.Baid, cancellationToken);

        if (user is null)
        {
            return ApiResult.Failed<bool>("User not found");
        }

        var credential = user.Credential;
        if (credential is null || credential.Password == string.Empty)
        {
            return ApiResult.Failed<bool>("User not registered");
        }

        if (!BCrypt.Net.BCrypt.Verify(request.OldPassword, credential.Password))
        {
            return ApiResult.Failed<bool>("Wrong old password");
        }

        var salt = BCrypt.Net.BCrypt.GenerateSalt();
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.NewPassword, salt);

        credential.Password = hashedPassword;
        credential.Salt = salt;

        await context.SaveChangesAsync(cancellationToken);
        return ApiResult.Succeed(true);
    }
}