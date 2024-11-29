namespace Server.Mappers;

[Mapper]
public static partial class DanDataMappers
{
    public static partial GetDanOdaiResponse.OdaiData To3906OdaiData(DanData data);
    
    [SuppressMessage("Mapper", "RMG020:Source member is not mapped to any target member")]
    public static partial Models.v3209.GetDanOdaiResponse.OdaiData To3209OdaiData(DanData data);
}