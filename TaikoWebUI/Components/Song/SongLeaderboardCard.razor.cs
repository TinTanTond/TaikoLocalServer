using Microsoft.Extensions.Options;
using TaikoWebUI.Settings;

namespace TaikoWebUI.Components.Song;
using LeaderBoard = PaginatedResult<SongLeaderboardEntry>;
public partial class SongLeaderboardCard
{
    [Inject]
    IOptions<WebUiSettings> UiSettings { get; set; } = default!;
    
    [Parameter]
    public int SongId { get; set; }
    
    [Parameter]
    public int Baid { get; set; }

    [Parameter] 
    public Difficulty Difficulty { get; set; } = Difficulty.None;
    
    private LeaderBoard? response = null;
    private List<SongLeaderboardEntry> LeaderboardScores { get; set; } = [];
    private int TotalRows { get; set; } = 0;
    private string SelectedDifficulty { get; set; } = "None";
    private bool isPaginationEnabled = true;
    private int TotalPages { get; set; } = 0;
    private bool isLoading = true;
    private int currentPage = 1;
    private int pageSize = 10;
    
    protected override void OnInitialized()
    {
        base.OnInitialized();
        
        if (UiSettings.Value.SongLeaderboardSettings.DisablePagination)
        {
            isPaginationEnabled = false;
        }

        if (UiSettings.Value.SongLeaderboardSettings.PageSize > 200 |
            UiSettings.Value.SongLeaderboardSettings.PageSize <= 0)
        {
            Console.WriteLine("Invalid LeaderboardSettings.PageSize value in appsettings.json. The value must be between 1 and 200. Defaulting to 10.");
        }
        
        if (UiSettings.Value.SongLeaderboardSettings.PageSize > 0 & UiSettings.Value.SongLeaderboardSettings.PageSize <= 200)
        {
            pageSize = UiSettings.Value.SongLeaderboardSettings.PageSize;
        }
    }
    
    private async Task GetLeaderboardData()
    {
        isLoading = true;
        var request = new GetSongLeaderboardRequest
        {
            SongId = (uint)SongId,
            Difficulty = Difficulty,
            Page = currentPage,
            Limit = pageSize
        };
        
        response = await Client.GetFromJsonAsync<LeaderBoard>($"api/SongLeaderboard/{(uint)SongId}");
        response.ThrowIfNull();
        
        LeaderboardScores.Clear();
        LeaderboardScores.AddRange(response.Data);
        
        // set TotalPages
        TotalPages = response.TotalPages;
    
        if (response.Current != null 
            && LeaderboardScores.All(x => x.Baid != response.Current.Baid) 
            && (LeaderboardScores.Count == 0 || response.Current.Rank >= LeaderboardScores[0].Rank))
        {
            LeaderboardScores.Add(new SongLeaderboardEntry()); // Add an empty row
            LeaderboardScores.Add(response.Current);
        }

        TotalRows = LeaderboardScores.Count;
        isLoading = false;
    }
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        
        // get highScoresTab from LocalStorage
        var songPageDifficulty = await LocalStorage.GetItemAsync<string>("songPageDifficulty");
        
        if (songPageDifficulty != null)
        {
            SelectedDifficulty = songPageDifficulty;
            Difficulty = Enum.Parse<Difficulty>(SelectedDifficulty);
        } 
        else
        {
            // set default difficulty to Easy
            SelectedDifficulty = Difficulty.Easy.ToString();
            Difficulty = Difficulty.Easy;
        }
        
        await GetLeaderboardData();
        
        isLoading = false;
    }
    
    private async Task OnDifficultyChange(string difficulty = "None")
    {
        isLoading = true;
        SelectedDifficulty = difficulty;
        Difficulty = Enum.Parse<Difficulty>(SelectedDifficulty);
        
        await LocalStorage.SetItemAsync("songPageDifficulty", SelectedDifficulty);
        await GetLeaderboardData();
        
        currentPage = 1;
        isLoading = false;
    }
    
    private async Task OnPageChange(int page)
    {
        currentPage = page;
        await GetLeaderboardData();
    }


    private Task UserChanged(SongLeaderboardEntry leaderboard)
    {
        NavigationManager.NavigateTo($"/Users/{leaderboard.Baid}/Songs/{SongId}", forceLoad: true);
        return Task.CompletedTask;
    }

    private string GetActiveRowClass(SongLeaderboardEntry leaderboard, int index)
    {
        return leaderboard.Baid == Baid ? "is-current-user" : "";
    }
}