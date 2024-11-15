using System.Text.Json.Serialization;

namespace TaikoLocalServer.Models;

public class MusicOrderEntry
{
    [JsonPropertyName("uniqueId")]
    public uint SongId { get; set; }
}