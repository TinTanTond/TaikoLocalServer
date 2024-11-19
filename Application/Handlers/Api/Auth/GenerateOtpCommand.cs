namespace Application.Handlers.Api.Auth;

public record GenerateOtpCommand(uint Baid) : IRequest<ApiResult<string>>;

public class GenerateOtpCommandHandler(ILogger<GenerateOtpCommandHandler> logger)
    : IRequestHandler<GenerateOtpCommand, ApiResult<string>>
{
    public Task<ApiResult<string>> Handle(GenerateOtpCommand request, CancellationToken cancellationToken)
    {
        var totp = TotpUtils.MakeTotp(request.Baid);
        var otp = totp.ComputeTotp();
        return Task.FromResult(ApiResult.Succeed(otp));
    }
}