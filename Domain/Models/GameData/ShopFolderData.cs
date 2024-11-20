using System.Text.Json.Serialization;

namespace Domain.Models.GameData;

public class ShopFolderData
{
    [JsonPropertyName("songNo")] public uint SongNo { get; set; }
    
    [JsonPropertyName("type")] public uint Type { get; set; }
    
    [JsonPropertyName("price")] public uint Price { get; set; }
}