using System.Text.Json.Serialization;

namespace Domain.Models;

public class MusicInfos
{
	[JsonPropertyName("items")]
	public List<MusicInfoEntry> MusicInfoEntries { get; set; } = new();
}