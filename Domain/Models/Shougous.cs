using System.Text.Json.Serialization;

namespace Domain.Models;

public class Shougous
{
	[JsonPropertyName("items")]
	public List<ShougouEntry> ShougouEntries { get; set; } = new();
}