using System.Text.Json.Serialization;

namespace Domain.Models.GameData;

public class WordList
{
    [JsonPropertyName("items")]
    public List<WordListEntry> WordListEntries { get; set; } = new();
}