namespace Server.Mappers;

[Mapper]
public static partial class MyDonEntryMappers
{
    [SuppressMessage("Mapper", "RMG020:Source member is not mapped to any target member")]
    [SuppressMessage("Mapper", "RMG012:Source member was not found for target member")]

    public static partial MydonEntryResponse MapTo3906(CommonMyDonEntryResponse response);
    
    [SuppressMessage("Mapper", "RMG020:Source member is not mapped to any target member")]
    public static partial Models.v3209.MydonEntryResponse MapTo3209(CommonMyDonEntryResponse response);
}