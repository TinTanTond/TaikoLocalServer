namespace Application.Handlers.Game;

public record VerifyQrQuery(string Serial): IRequest<CommonVerifyQrResponse>;

public class VerifyQrQueryHandler(IGameDataService gameDataService)
    : IRequestHandler<VerifyQrQuery, CommonVerifyQrResponse>
{
    public Task<CommonVerifyQrResponse> Handle(VerifyQrQuery request, CancellationToken cancellationToken)
    {
        var qrCodeDataDictionary = gameDataService.GetQRCodeDataDictionary();

        qrCodeDataDictionary.TryGetValue(request.Serial, out var qrCodeId);

        return Task.FromResult(qrCodeId == 0 ? 
            new CommonVerifyQrResponse(false, 0) : new CommonVerifyQrResponse(true, qrCodeId));
    }
}