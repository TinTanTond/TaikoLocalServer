using System.Text.Json.Serialization;

namespace Domain.Models;

public class QRCodeData
{
    [JsonPropertyName("serial")] public string Serial { get; set; } = null!;

    [JsonPropertyName("id")] public uint Id { get; set; }
}