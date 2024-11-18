using System.Text.Json.Serialization;

namespace Domain.Models;

public class MusicOrder
{
    [JsonPropertyName("items")]
    public List<MusicOrderEntry> Order { get; set; } = new();
}