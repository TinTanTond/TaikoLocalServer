using Microsoft.Extensions.Options;
using SharedProject.Models;
using SharedProject.Utils;
using TaikoLocalServer.Filters;
using TaikoLocalServer.Settings;

namespace TaikoLocalServer.Controllers.Api;

[ApiController]
[Route("/api/[controller]")]
public class UserSettingsController(IUserDatumService userDatumService, IAuthService authService, 
    IOptions<AuthSettings> settings) : BaseController<UserSettingsController>
{
    private readonly AuthSettings authSettings = settings.Value;

    [HttpGet]
    [ServiceFilter(typeof(AuthorizeIfRequiredAttribute))]
    public async Task<ActionResult<List<UserSetting>>> GetAllUserSetting()
    {
        if (authSettings.AuthenticationRequired)
        {
            var tokenInfo = authService.ExtractTokenInfo(HttpContext);
            if (tokenInfo is null)
            {
                return Unauthorized();
            }
            
            if (!tokenInfo.Value.isAdmin)
            {
                return Forbid();
            }
        }
        
        var users = await userDatumService.GetAllUserDatum();
        
        var response = new List<UserSetting>();
        
        foreach (var user in users)
        {
            List<List<uint>> costumeUnlockData = 
                [user.UnlockedKigurumi, user.UnlockedHead, user.UnlockedBody, user.UnlockedFace, user.UnlockedPuchi];

            var unlockedTitle = user.TitleFlgArray
                .ToList();

            for (var i = 0; i < 5; i++)
            {
                if (!costumeUnlockData[i].Contains(0))
                {
                    costumeUnlockData[i].Add(0);
                }
            }

            var userSetting = new UserSetting
            {
                Baid = user.Baid,
                AchievementDisplayDifficulty = user.AchievementDisplayDifficulty,
                DisplayAchievement = user.DisplayAchievement,
                DisplayDan = user.DisplayDan,
                DifficultySettingCourse = user.DifficultySettingCourse,
                DifficultySettingStar = user.DifficultySettingStar,
                DifficultySettingSort = user.DifficultySettingSort,
                IsVoiceOn = user.IsVoiceOn,
                IsSkipOn = user.IsSkipOn,
                NotesPosition = user.NotesPosition,
                PlaySetting = PlaySettingConverter.ShortToPlaySetting(user.OptionSetting),
                SelectedToneId = user.SelectedToneId,
                MyDonName = user.MyDonName,
                MyDonNameLanguage = user.MyDonNameLanguage,
                Title = user.Title,
                TitlePlateId = user.TitlePlateId,
                CurrentKigurumi = user.CurrentKigurumi,
                CurrentHead = user.CurrentHead,
                CurrentBody = user.CurrentBody,
                CurrentFace = user.CurrentFace,
                CurrentPuchi = user.CurrentPuchi,
                UnlockedKigurumi = costumeUnlockData[0],
                UnlockedHead = costumeUnlockData[1],
                UnlockedBody = costumeUnlockData[2],
                UnlockedFace = costumeUnlockData[3],
                UnlockedPuchi = costumeUnlockData[4],
                UnlockedTitle = unlockedTitle,
                ColorBody = user.ColorBody,
                ColorFace = user.ColorFace,
                ColorLimb = user.ColorLimb,
                LastPlayDateTime = user.LastPlayDatetime
            };
            response.Add(userSetting);
        }
        
        return Ok(response);
    }
    
    
    [HttpGet("{baid}")]
    [ServiceFilter(typeof(AuthorizeIfRequiredAttribute))]
    public async Task<ActionResult<UserSetting>> GetUserSetting(uint baid)
    {
        if (authSettings.AuthenticationRequired)
        {
            var tokenInfo = authService.ExtractTokenInfo(HttpContext);
            if (tokenInfo is null)
            {
                return Unauthorized();
            }
            
            if (tokenInfo.Value.baid != baid && !tokenInfo.Value.isAdmin)
            {
                return Forbid();
            }
        }
        
        var user = await userDatumService.GetFirstUserDatumOrNull(baid);

        if (user is null)
        {
            return NotFound();
        }

        List<List<uint>> costumeUnlockData = 
            [user.UnlockedKigurumi, user.UnlockedHead, user.UnlockedBody, user.UnlockedFace, user.UnlockedPuchi];

        var unlockedTitle = user.TitleFlgArray
            .ToList();

        for (var i = 0; i < 5; i++)
        {
            if (!costumeUnlockData[i].Contains(0))
            {
                costumeUnlockData[i].Add(0);
            }
        }

        var response = new UserSetting
        {
            Baid = user.Baid,
            AchievementDisplayDifficulty = user.AchievementDisplayDifficulty,
            DisplayAchievement = user.DisplayAchievement,
            DisplayDan = user.DisplayDan,
            DifficultySettingCourse = user.DifficultySettingCourse,
            DifficultySettingStar = user.DifficultySettingStar,
            DifficultySettingSort = user.DifficultySettingSort,
            IsVoiceOn = user.IsVoiceOn,
            IsSkipOn = user.IsSkipOn,
            NotesPosition = user.NotesPosition,
            PlaySetting = PlaySettingConverter.ShortToPlaySetting(user.OptionSetting),
            SelectedToneId = user.SelectedToneId,
            MyDonName = user.MyDonName,
            MyDonNameLanguage = user.MyDonNameLanguage,
            Title = user.Title,
            TitlePlateId = user.TitlePlateId,
            CurrentKigurumi = user.CurrentKigurumi,
            CurrentHead = user.CurrentHead,
            CurrentBody = user.CurrentBody,
            CurrentFace = user.CurrentFace,
            CurrentPuchi = user.CurrentPuchi,
            UnlockedKigurumi = costumeUnlockData[0],
            UnlockedHead = costumeUnlockData[1],
            UnlockedBody = costumeUnlockData[2],
            UnlockedFace = costumeUnlockData[3],
            UnlockedPuchi = costumeUnlockData[4],
            UnlockedTitle = unlockedTitle,
            ColorBody = user.ColorBody,
            ColorFace = user.ColorFace,
            ColorLimb = user.ColorLimb,
            LastPlayDateTime = user.LastPlayDatetime
        };
        return Ok(response);
    }

    [HttpPost("{baid}")]
    [ServiceFilter(typeof(AuthorizeIfRequiredAttribute))]
    public async Task<IActionResult> SaveUserSetting(uint baid, UserSetting userSetting)
    {
        if (authSettings.AuthenticationRequired)
        {
            var tokenInfo = authService.ExtractTokenInfo(HttpContext);
            if (tokenInfo is null)
            {
                return Unauthorized();
            }
            
            if (tokenInfo.Value.baid != baid && !tokenInfo.Value.isAdmin)
            {
                return Forbid();
            }
        }
        
        var user = await userDatumService.GetFirstUserDatumOrNull(baid);

        if (user is null)
        {
            return NotFound();
        }

        user.IsSkipOn = userSetting.IsSkipOn;
        user.IsVoiceOn = userSetting.IsVoiceOn;
        user.DisplayAchievement = userSetting.DisplayAchievement;
        user.DisplayDan = userSetting.DisplayDan;
        user.DifficultySettingCourse = userSetting.DifficultySettingCourse;
        user.DifficultySettingStar = userSetting.DifficultySettingStar;
        user.DifficultySettingSort = userSetting.DifficultySettingSort;
        user.NotesPosition = userSetting.NotesPosition;
        user.SelectedToneId = userSetting.SelectedToneId;
        user.AchievementDisplayDifficulty = userSetting.AchievementDisplayDifficulty;
        user.OptionSetting = PlaySettingConverter.PlaySettingToShort(userSetting.PlaySetting);
        user.MyDonName = userSetting.MyDonName;
        user.MyDonNameLanguage = userSetting.MyDonNameLanguage;
        user.Title = userSetting.Title;
        user.TitlePlateId = userSetting.TitlePlateId;
        user.ColorBody = userSetting.ColorBody;
        user.ColorFace = userSetting.ColorFace;
        user.ColorLimb = userSetting.ColorLimb;
        user.CurrentKigurumi = userSetting.CurrentKigurumi;
        user.CurrentHead = userSetting.CurrentHead;
        user.CurrentBody = userSetting.CurrentBody;
        user.CurrentFace = userSetting.CurrentFace;
        user.CurrentPuchi = userSetting.CurrentPuchi;

        // If a locked tone is selected, unlock it
        var toneFlg = user.ToneFlgArray;
        toneFlg = toneFlg.Append(0u).Append(userSetting.SelectedToneId).Distinct().ToList();

        user.ToneFlgArray = toneFlg;

        await userDatumService.UpdateUserDatum(user);

        return NoContent();
    }

}