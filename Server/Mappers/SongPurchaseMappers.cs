namespace Server.Mappers;

[Mapper]
public static partial class SongPurchaseMappers
{
    public static partial SongPurchaseResponse MapTo3906(CommonSongPurchaseResponse response);
    
    public static partial Models.v3209.SongPurchaseResponse MapTo3209(CommonSongPurchaseResponse response);
    
    
    [SuppressMessage("Mapper", "RMG020:Source member is not mapped to any target member")]
    public static partial PurchaseSongCommand MapToCommand(SongPurchaseRequest request);
    
    
    [SuppressMessage("Mapper", "RMG020:Source member is not mapped to any target member")]
    public static partial PurchaseSongCommandCN MapToCommand(Models.v3209.SongPurchaseRequest request);
}