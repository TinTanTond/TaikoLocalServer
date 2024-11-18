using System.Text.Json.Serialization;

namespace Domain.Models;

public class Neiros
{
	[JsonPropertyName("items")]
	public List<NeiroEntry> NeiroEntries { get; set; } = new();
}