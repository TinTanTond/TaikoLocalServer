using System.Text.Json.Serialization;

namespace Domain.Models.GameData;

public class Shougous
{
	[JsonPropertyName("items")]
	public List<ShougouEntry> ShougouEntries { get; set; } = new();
}