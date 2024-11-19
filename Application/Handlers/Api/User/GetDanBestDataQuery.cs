using Application.Mappers;

namespace Application.Handlers.Api.Data;

public record GetDanBestDataQuery(uint Baid): IRequest<ApiResult<List<DanBestData>>>;

public class GetDanBestDataQueryHandler(ITaikoDbContext context, ILogger<GetDanBestDataQueryHandler> logger)
    : IRequestHandler<GetDanBestDataQuery, ApiResult<List<DanBestData>>>
{
    public async Task<ApiResult<List<DanBestData>>> Handle(GetDanBestDataQuery request, CancellationToken cancellationToken)
    {
        var danBestData = await context.DanScoreData.Where(datum => datum.Baid == request.Baid && datum.DanType == DanType.Normal)
            .Include(datum => datum.DanStageScoreData)
            .Select(d => DanDataMapper.MapToDanBestData(d))
            .ToListAsync(cancellationToken);
        return ApiResult.Succeed(danBestData);
    }
}