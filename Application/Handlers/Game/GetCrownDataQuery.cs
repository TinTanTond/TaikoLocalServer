using Domain.Settings;
using Microsoft.Extensions.Options;

namespace Application.Handlers.Game;

public record GetCrownDataQuery(uint Baid): IRequest<CommonCrownDataResponse>;

public class GetCrownDataQueryHandler(ITaikoDbContext context, IOptions<ServerSettings> options, ILogger<GetCrownDataQueryHandler> logger) 
    : IRequestHandler<GetCrownDataQuery, CommonCrownDataResponse>
{

    public async Task<CommonCrownDataResponse> Handle(GetCrownDataQuery request, CancellationToken cancellationToken)
    {
        var songBestData = await context.SongBestData.Where(datum => datum.Baid == request.Baid)
            .ToListAsync(cancellationToken);
        var songIdMax = options.Value.EnableMoreSongs ? Constants.MusicIdMaxExpanded : Constants.MusicIdMax;
        
        var crown = new ushort[songIdMax + 1];
        var dondafulCrown = new ushort[songIdMax + 1];

        for (var songId = 0; songId < songIdMax; songId++)
        {
            var id = songId;
            dondafulCrown[songId] = songBestData
                // Select song of this song id with dondaful crown 
                .Where(datum => datum.SongId    == id &&
                                datum.BestCrown == CrownType.Dondaful)
                // Calculate flag according to difficulty
                .Aggregate((byte)0, (flag, datum) => FlagCalculator.ComputeDondafulCrownFlag(flag, datum.Difficulty));

            crown[songId] = songBestData
                // Select song of this song id with clear or fc crown
                .Where(datum => datum.SongId == id &&
                                datum.BestCrown is CrownType.Clear or CrownType.Gold)
                // Calculate flag according to difficulty
                .Aggregate((ushort)0, (flag, datum) => FlagCalculator.ComputeCrownFlag(flag, datum.BestCrown, datum.Difficulty));
        }
        
        var response = new CommonCrownDataResponse(GZipBytesUtil.GetGZipBytes(crown), GZipBytesUtil.GetGZipBytes(dondafulCrown));

        return response;
    }
}