namespace Server.Mappers;

[Mapper]
public static partial class PlayResultMappers
{
    public static partial CommonPlayResultData Map(PlayResultDataRequest request);
    
    [SuppressMessage("Mapper", "RMG020:Source member is not mapped to any target member")]
    [SuppressMessage("Mapper", "RMG012:Source member was not found for target member")]

    public static partial CommonPlayResultData Map(Models.v3209.PlayResultDataRequest request);
}