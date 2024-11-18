using System.Text.Json.Serialization;

namespace Domain.Models;

public class ShougouEntry
{
	[JsonPropertyName("uniqueId")]
	public uint UniqueId { get; set; }
	
	[JsonPropertyName("rarity")]
	public uint Rarity { get; set; }
}