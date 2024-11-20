using Domain.Enums;

namespace Domain.Models;

public class UserSetting
{
    public uint Baid { get; set; }
    
    public uint SelectedToneId { get; set; }

    public bool DisplayAchievement { get; set; }

    public bool DisplayDan { get; set; }

    public uint DifficultySettingCourse { get; set; }

    public uint DifficultySettingStar { get; set; }

    public uint DifficultySettingSort { get; set; }

    public bool IsVoiceOn { get; set; }

    public bool IsSkipOn { get; set; }

    public Difficulty AchievementDisplayDifficulty { get; set; }

    public PlaySetting PlaySetting { get; set; } = new();

    public int NotesPosition { get; set; }

    public string MyDonName { get; set; } = string.Empty;

    public uint MyDonNameLanguage { get; set; }

    public string Title { get; set; } = string.Empty;

    public uint TitlePlateId { get; set; }

    public uint CurrentKigurumi { get; set; }

    public uint CurrentHead { get; set; }

    public uint CurrentBody { get; set; }

    public uint CurrentFace { get; set; }

    public uint CurrentPuchi { get; set; }

    public List<uint> UnlockedKigurumi { get; set; } = new();

    public List<uint> UnlockedHead { get; set; } = new();

    public List<uint> UnlockedBody { get; set; } = new();

    public List<uint> UnlockedFace { get; set; } = new();

    public List<uint> UnlockedPuchi { get; set; } = new();
    
    public List<uint> UnlockedTitle { get; set; } = new();

    public uint ColorFace { get; set; }

    public uint ColorBody { get; set; }

    public uint ColorLimb { get; set; }
    
    public DateTime LastPlayDatetime { get; set; }
}