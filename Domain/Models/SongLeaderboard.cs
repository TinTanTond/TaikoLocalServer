namespace Domain.Models;

public class SongLeaderboard
{
    public List<SongLeaderboardEntry> LeaderboardData { get; set; } = [];
    public SongLeaderboardEntry? UserScore { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalScores { get; set; }
}