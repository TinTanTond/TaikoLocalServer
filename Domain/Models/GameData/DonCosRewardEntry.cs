using System.Text.Json.Serialization;

namespace Domain.Models.GameData;

public class DonCosRewardEntry
{
	[JsonPropertyName("cosType")]
	public string CosType { get; set; } = null!;

	[JsonPropertyName("uniqueId")]
	public uint UniqueId { get; set; }
}