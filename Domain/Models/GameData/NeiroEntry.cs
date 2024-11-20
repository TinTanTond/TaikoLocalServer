using System.Text.Json.Serialization;

namespace Domain.Models.GameData;

public class NeiroEntry
{
	[JsonPropertyName("uniqueId")]
	public uint UniqueId { get; set; }
}