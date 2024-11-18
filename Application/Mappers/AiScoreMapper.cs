using Application.Models.Game;
using Riok.Mapperly.Abstractions;

namespace Application.Mappers;

[Mapper]
public static partial class AiScoreMapper
{
#pragma warning disable RMG020
    [MapProperty(nameof(AiScoreDatum.AiSectionScoreData), nameof(CommonAiScoreResponse.AryBestSectionDatas))]
    public static partial CommonAiScoreResponse MapToCommonAiScoreResponse(AiScoreDatum datum);
#pragma warning restore RMG020

    public static CommonAiScoreResponse MapAsSuccess(AiScoreDatum datum)
    {
        var response= MapToCommonAiScoreResponse(datum);
        response.Result = 1;
        return response;
    }
}