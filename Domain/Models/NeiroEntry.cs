using System.Text.Json.Serialization;

namespace Domain.Models;

public class NeiroEntry
{
	[JsonPropertyName("uniqueId")]
	public uint UniqueId { get; set; }
}