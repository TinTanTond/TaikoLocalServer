using System.Diagnostics.CodeAnalysis;
using Riok.Mapperly.Abstractions;

namespace Application.Mappers;

[Mapper]
[SuppressMessage("Mapper", "RMG020:Source member is not mapped to any target member")]
public static partial class AiScoreMapper
{
    [SuppressMessage("Mapper", "RMG012:Source member was not found for target member")]
    [MapProperty(nameof(AiScoreDatum.AiSectionScoreData), nameof(CommonAiScoreResponse.AryBestSectionDatas))]
    public static partial CommonAiScoreResponse MapToCommonAiScoreResponse(AiScoreDatum datum);

    public static CommonAiScoreResponse MapAsSuccess(AiScoreDatum datum)
    {
        var response= MapToCommonAiScoreResponse(datum);
        response.Result = 1;
        return response;
    }
}