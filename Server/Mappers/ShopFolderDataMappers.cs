namespace Server.Mappers;

[Mapper]
public static partial class ShopFolderDataMappers
{
    public static partial GetShopFolderResponse MapTo3906(CommonGetShopFolderResponse response);
    
    [SuppressMessage("Mapper", "RMG020:Source member is not mapped to any target member")]
    public static partial Models.v3209.GetShopFolderResponse MapTo3209(CommonGetShopFolderResponse response);
}