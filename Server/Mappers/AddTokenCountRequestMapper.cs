namespace Server.Mappers;

[Mapper]
public static partial class AddTokenCountRequestMapper
{
    [SuppressMessage("Mapper", "RMG020:Source member is not mapped to any target member")]
    public static partial CommonAddTokenCountRequest Map(AddTokenCountRequest request);
    
    [SuppressMessage("Mapper", "RMG020:Source member is not mapped to any target member")]
    public static partial CommonAddTokenCountRequest Map(Models.v3209.AddTokenCountRequest request);
}