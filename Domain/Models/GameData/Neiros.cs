using System.Text.Json.Serialization;

namespace Domain.Models.GameData;

public class Neiros
{
	[JsonPropertyName("items")]
	public List<NeiroEntry> NeiroEntries { get; set; } = new();
}