namespace Application.Handlers.Game;

public record GetAiDataQuery(uint Baid) : IRequest<CommonAiDataResponse>;

public class GetAiDataQueryHandler(ITaikoDbContext context, ILogger<GetAiDataQueryHandler> logger)
    : IRequestHandler<GetAiDataQuery, CommonAiDataResponse>
{
    public async Task<CommonAiDataResponse> Handle(GetAiDataQuery request, CancellationToken cancellationToken)
    {
        var user = await context.UserData.FirstOrDefaultAsync(datum => datum.Baid == request.Baid, cancellationToken);
        user.ThrowIfNull($"User with baid {request.Baid} does not exist!");
        var response = new CommonAiDataResponse
        {
            Result = 1,
            TotalWinnings = (uint)user.AiWinCount,
            InputMedian = "1",
            InputVariance = "0"
        };
        return response;
    }
}
