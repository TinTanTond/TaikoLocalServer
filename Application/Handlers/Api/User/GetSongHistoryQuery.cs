using Application.Mappers;

namespace Application.Handlers.Api.User;

public record GetSongHistoryQuery(uint Baid) : IRequest<ApiResult<List<SongHistoryData>>>;

public class GetSongHistoryQueryHandler(ITaikoDbContext context, ILogger<GetSongHistoryQueryHandler> logger)
    : IRequestHandler<GetSongHistoryQuery, ApiResult<List<SongHistoryData>>>
{
    public async Task<ApiResult<List<SongHistoryData>>> Handle(GetSongHistoryQuery request, CancellationToken cancellationToken)
    {
        var user = await context.UserData
            .FirstOrDefaultAsync(d => d.Baid == request.Baid, cancellationToken);
        if (user is null)
        {
            return ApiResult.Failed<List<SongHistoryData>>("User not found");
        }

        var playLogs = await context.SongPlayData
            .Where(datum => datum.Baid == request.Baid)
            .ToListAsync(cancellationToken);

        var songHistory = playLogs.Select(SongHistoryDataMapper.ToSongHistoryData).ToList();

        var favoriteSongs = user.FavoriteSongsArray.ToHashSet();
        foreach (var song in songHistory.Where(song => favoriteSongs.Contains(song.SongId)))
        {
            song.IsFavorite = true;
        }

        return ApiResult.Succeed(songHistory);
    }
}