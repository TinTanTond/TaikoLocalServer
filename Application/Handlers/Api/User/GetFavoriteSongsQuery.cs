namespace Application.Handlers.Api.User;

public record GetFavoriteSongsQuery(uint Baid): IRequest<ApiResult<List<uint>>>;

public class GetFavoriteSongsQueryHandler(ITaikoDbContext context, ILogger<GetFavoriteSongsQueryHandler> logger)
    : IRequestHandler<GetFavoriteSongsQuery, ApiResult<List<uint>>>
{
    public async Task<ApiResult<List<uint>>> Handle(GetFavoriteSongsQuery request, CancellationToken cancellationToken)
    {
        var user = await context.UserData.FindAsync([request.Baid], cancellationToken);
        if (user is null)
        {
            return ApiResult.Failed<List<uint>>("User not found");
        }
        
        var favoriteSongs = user.FavoriteSongsArray;
        return ApiResult.Succeed(favoriteSongs);
    }
}