using Domain.Settings;
using Microsoft.Extensions.Options;

namespace Application.Handlers.Game;

public record GetScoreRankQuery(uint Baid): IRequest<CommonScoreRankResponse>;

public class GetScoreRankQueryHandler(ITaikoDbContext context, IOptions<ServerSettings> options, ILogger<GetScoreRankQueryHandler> logger) 
    : IRequestHandler<GetScoreRankQuery, CommonScoreRankResponse>
{
    public async Task<CommonScoreRankResponse> Handle(GetScoreRankQuery request, CancellationToken cancellationToken)
    {
        var songIdMax = options.Value.EnableMoreSongs ? Constants.MusicIdMaxExpanded : Constants.MusicIdMax;
        var kiwamiScores = new byte[songIdMax   + 1];
        var miyabiScores = new ushort[songIdMax + 1];
        var ikiScores = new ushort[songIdMax    + 1];
        var songBestData = await context.SongBestData.Where(datum => datum.Baid == request.Baid)
            .ToListAsync(cancellationToken);

        for (var songId = 0; songId < songIdMax; songId++)
        {
            var id = songId;
            kiwamiScores[songId] = songBestData
                .Where(datum => datum.SongId        == id &&
                                datum.BestScoreRank == ScoreRank.Dondaful)
                .Aggregate((byte)0, (flag, datum) => FlagCalculator.ComputeKiwamiScoreRankFlag(flag, datum.Difficulty));

            ikiScores[songId] = songBestData
                .Where(datum => datum.SongId == id &&
                                datum.BestScoreRank is ScoreRank.White or ScoreRank.Bronze or ScoreRank.Silver)
                .Aggregate((ushort)0, (flag, datum) => FlagCalculator.ComputeMiyabiOrIkiScoreRank(flag, datum.BestScoreRank, datum.Difficulty));

            miyabiScores[songId] = songBestData
                .Where(datum => datum.SongId == id &&
                                datum.BestScoreRank is ScoreRank.Gold or ScoreRank.Purple or ScoreRank.Sakura)
                .Aggregate((ushort)0, (flag, datum) => FlagCalculator.ComputeMiyabiOrIkiScoreRank(flag, datum.BestScoreRank, datum.Difficulty));
        }

        var response = new CommonScoreRankResponse(
            GZipBytesUtil.GetGZipBytes(ikiScores),
            GZipBytesUtil.GetGZipBytes(kiwamiScores),
            GZipBytesUtil.GetGZipBytes(miyabiScores)
        );
        return response;
    }
}

