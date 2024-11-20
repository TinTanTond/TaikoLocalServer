using System.Diagnostics.CodeAnalysis;
using Riok.Mapperly.Abstractions;

namespace Application.Mappers;

[Mapper(AutoUserMappings = false)]
public static partial class UserSettingMapper
{
    [SuppressMessage("Mapper", "RMG020:Source member is not mapped to any target member")]
    [MapProperty(nameof(UserDatum.TitleFlgArray), nameof(UserSetting.UnlockedTitle))]
    [MapProperty(nameof(UserDatum.OptionSetting), nameof(UserSetting.PlaySetting), Use = nameof(ShortToPlaySetting))]
    [MapProperty(nameof(UserDatum.UnlockedKigurumi), nameof(UserSetting.UnlockedKigurumi), Use = nameof(FixUnlock))]
    [MapProperty(nameof(UserDatum.UnlockedBody), nameof(UserSetting.UnlockedBody), Use = nameof(FixUnlock))]
    [MapProperty(nameof(UserDatum.UnlockedFace), nameof(UserSetting.UnlockedFace), Use = nameof(FixUnlock))]
    [MapProperty(nameof(UserDatum.UnlockedHead), nameof(UserSetting.UnlockedHead), Use = nameof(FixUnlock))]
    [MapProperty(nameof(UserDatum.UnlockedPuchi), nameof(UserSetting.UnlockedPuchi), Use = nameof(FixUnlock))]
    public static partial UserSetting MapToUserSetting(UserDatum user);

    public static PlaySetting ShortToPlaySetting(short option)
    {
        return PlaySettingConverter.ShortToPlaySetting(option);
    }
    
    public static List<uint> FixUnlock(List<uint> unlock)
    {
        if (!unlock.Contains(0))
        {
            unlock.Add(0);
        }

        return unlock;
    }
}