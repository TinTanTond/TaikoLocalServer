using System.Text.Json.Serialization;

namespace Domain.Models.GameData;

public class MusicInfos
{
	[JsonPropertyName("items")]
	public List<MusicInfoEntry> MusicInfoEntries { get; set; } = new();
}