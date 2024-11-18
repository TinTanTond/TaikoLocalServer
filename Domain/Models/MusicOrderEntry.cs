using System.Text.Json.Serialization;

namespace Domain.Models;

public class MusicOrderEntry
{
    [JsonPropertyName("uniqueId")]
    public uint SongId { get; set; }
}