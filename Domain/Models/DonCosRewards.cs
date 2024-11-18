using System.Text.Json.Serialization;

namespace Domain.Models;

public class DonCosRewards
{
	[JsonPropertyName("items")]
	public List<DonCosRewardEntry> DonCosRewardEntries { get; set; } = new();
}