using System.Text.Json.Serialization;

namespace Domain.Models.GameData;

public class MusicOrder
{
    [JsonPropertyName("items")]
    public List<MusicOrderEntry> Order { get; set; } = new();
}