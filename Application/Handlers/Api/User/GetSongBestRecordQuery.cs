// File: Application/Handlers/Api/User/GetSongBestRecordsQuery.cs

using Application.Mappers;

namespace Application.Handlers.Api.User;

public record GetSongBestRecordsQuery(uint Baid) : IRequest<ApiResult<List<SongBestData>>>;

public class GetSongBestRecordsQueryHandler(ITaikoDbContext context, ILogger<GetSongBestRecordsQueryHandler> logger)
    : IRequestHandler<GetSongBestRecordsQuery, ApiResult<List<SongBestData>>>
{
    public async Task<ApiResult<List<SongBestData>>> Handle(GetSongBestRecordsQuery request, 
        CancellationToken cancellationToken)
    {
        var user = await context.UserData
            .FirstOrDefaultAsync(d => d.Baid == request.Baid, cancellationToken);
        if (user is null)
        {
            return ApiResult.Failed<List<SongBestData>>("User not found");
        }

        var songBestRecords = await context.SongBestData
            .Where(datum => datum.Baid == request.Baid)
            .Select(datum => SongBestDataMapper.ToSongBestData(datum))
            .ToListAsync(cancellationToken);

        var songPlayData = await context.SongPlayData
            .Where(datum => datum.Baid == request.Baid)
            .ToListAsync(cancellationToken);

        foreach (var songBestData in songBestRecords)
        {
            var songPlayLogs = songPlayData
                .Where(datum => datum.SongId == songBestData.SongId && datum.Difficulty == songBestData.Difficulty)
                .ToList();
            songBestData.PlayCount = songPlayLogs.Count;
            songBestData.ClearCount = songPlayLogs.Count(datum => datum.Crown >= CrownType.Clear);
            songBestData.FullComboCount = songPlayLogs.Count(datum => datum.Crown >= CrownType.Gold);
            songBestData.PerfectCount = songPlayLogs.Count(datum => datum.Crown >= CrownType.Dondaful);
        }

        var favoriteSongs = user.FavoriteSongsArray.ToHashSet();
        foreach (var songBestRecord in songBestRecords.Where(songBestRecord => favoriteSongs.Contains(songBestRecord.SongId)))
        {
            songBestRecord.IsFavorite = true;
        }

        foreach (var songBestRecord in songBestRecords)
        {
            songBestRecord.RecentPlayData = songPlayData
                .Where(datum => datum.SongId == songBestRecord.SongId && datum.Difficulty == songBestRecord.Difficulty)
                .Select(SongPlayDataMapper.MapToDto)
                .ToList();
        }

        return ApiResult.Succeed(songBestRecords);
    }
}