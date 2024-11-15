using System.Text.Json.Serialization;

namespace TaikoLocalServer.Models;

public class WordList
{
    [JsonPropertyName("items")]
    public List<WordListEntry> WordListEntries { get; set; } = new();
}