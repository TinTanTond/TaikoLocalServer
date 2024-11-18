namespace Application.Handlers.Game;

public record GetDanOdaiQuery(uint[] DanIds, uint Type) : IRequest<List<DanData>>;

public class GetDanOdaiQueryHandler(IGameDataService gameDataService) : IRequestHandler<GetDanOdaiQuery, List<DanData>>
{
    private readonly IGameDataService gameDataService = gameDataService;

    public Task<List<DanData>> Handle(GetDanOdaiQuery request, CancellationToken cancellationToken)
    {
        var type = (DanType)request.Type;
        type.Throw().IfOutOfRange();
        var danDataList = new List<DanData>();
        switch (type)
        {
            case DanType.Normal:
                var danDataDictionary = gameDataService.GetCommonDanDataDictionary();

                foreach (var danId in request.DanIds)
                {
                    if (danDataDictionary.TryGetValue(danId, out var danData))
                    {
                        danDataList.Add(danData);
                    }
                }
                break;
            case DanType.Gaiden:
                var gaidenDataDictionary = gameDataService.GetCommonGaidenDataDictionary();
                
                foreach (var danId in request.DanIds)
                {
                    if (gaidenDataDictionary.TryGetValue(danId, out var danData))
                    {
                        danDataList.Add(danData);
                    }
                }
                break;
            default:
                throw new ApplicationException("Impossible");
        }

        return Task.FromResult(danDataList);
    }
}