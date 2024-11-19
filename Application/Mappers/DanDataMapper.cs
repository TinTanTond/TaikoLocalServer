using Riok.Mapperly.Abstractions;

namespace Application.Mappers;

[Mapper]
public static partial class DanDataMapper
{
    [MapProperty(nameof(DanScoreDatum.DanStageScoreData), nameof(DanBestData.DanBestStageDataList))]
    [MapperIgnoreSource(nameof(DanScoreDatum.Baid))]
    [MapperIgnoreSource(nameof(DanScoreDatum.DanType))]
    [MapperIgnoreSource(nameof(DanScoreDatum.ArrivalSongCount))]
    [MapperIgnoreSource(nameof(DanScoreDatum.Ba))]
    public static partial DanBestData MapToDanBestData(DanScoreDatum datum);
    
    [MapperIgnoreSource(nameof(datum.Baid))]
    [MapperIgnoreSource(nameof(datum.DanId))]
    [MapperIgnoreSource(nameof(datum.DanType))]
    [MapperIgnoreSource(nameof(datum.Parent))]
    public static partial DanBestStageData MapToDanBestStageData(DanStageScoreDatum datum);
}