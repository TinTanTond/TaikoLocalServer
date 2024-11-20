using System.Text.Json.Serialization;

namespace Domain.Models.GameData;

public class DonCosRewards
{
	[JsonPropertyName("items")]
	public List<DonCosRewardEntry> DonCosRewardEntries { get; set; } = new();
}