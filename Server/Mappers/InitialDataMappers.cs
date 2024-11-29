namespace Server.Mappers;

[Mapper]
public static partial class InitialDataMappers
{
    [SuppressMessage("Mapper", "RMG020:Source member is not mapped to any target member")]
    public static partial InitialdatacheckResponse MapTo3906(CommonInitialDataCheckResponse response);
    
    public static partial Models.v3209.InitialdatacheckResponse MapTo3209(CommonInitialDataCheckResponse response);
}