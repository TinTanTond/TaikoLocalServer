using System.Text.Json.Serialization;

namespace Domain.Models;

public class MovieData
{
	[JsonPropertyName("movie_id")]
	public uint MovieId { get; set; }

	[JsonPropertyName("enable_days")]
	public uint EnableDays { get; set; }
}