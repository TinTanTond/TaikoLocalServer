using System.Text.Json.Serialization;

namespace Domain.Models.GameData;

public class MusicOrderEntry
{
    [JsonPropertyName("uniqueId")]
    public uint SongId { get; set; }
}