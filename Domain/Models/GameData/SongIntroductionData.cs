using System.Text.Json.Serialization;

namespace Domain.Models.GameData;

public class SongIntroductionData : IVerupNo
{
    [JsonPropertyName("setId")]
    public uint SetId { get; set; }

    [JsonPropertyName("verupNo")]
    public uint VerupNo { get; set; }

    [JsonPropertyName("mainSongNo")]
    public uint MainSongNo { get; set; }

    [JsonPropertyName("subSongNo")]
    public uint[]? SubSongNoes { get; set; }
}