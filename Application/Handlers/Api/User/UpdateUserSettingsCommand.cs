using Application.Mappers;

namespace Application.Handlers.Api.User;

public record UpdateUserSettingsCommand(uint Baid, UserSetting UserSetting): IRequest<ApiResult<bool>>;

public class UpdateUserSettingsCommandHandler(ITaikoDbContext context)
    : IRequestHandler<UpdateUserSettingsCommand, ApiResult<bool>>
{
    public async Task<ApiResult<bool>> Handle(UpdateUserSettingsCommand request, CancellationToken cancellationToken)
    {
        var user = await context.UserData.FirstOrDefaultAsync(u => u.Baid == request.Baid, cancellationToken);
        if (user is null)
        {
            return ApiResult.Failed<bool>("User not found!");
        }
        UserSettingMapper.UpdateUserSetting(request.UserSetting, user);

        var toneFlg = user.ToneFlgArray;
        toneFlg = toneFlg.Append(0u).Append(request.UserSetting.SelectedToneId).Distinct().ToList();

        user.ToneFlgArray = toneFlg;
        await context.SaveChangesAsync(cancellationToken);
        return ApiResult.Succeed(true);
    }
}