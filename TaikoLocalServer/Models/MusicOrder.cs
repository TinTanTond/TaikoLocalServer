using System.Text.Json.Serialization;

namespace TaikoLocalServer.Models;

public class MusicOrder
{
    [JsonPropertyName("items")]
    public List<MusicOrderEntry> Order { get; set; } = new();
}