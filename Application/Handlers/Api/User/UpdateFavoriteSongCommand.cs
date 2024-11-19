namespace Application.Handlers.Api.User;

public record UpdateFavoriteSongCommand(uint Baid, uint SongId, bool IsFavorite): IRequest<ApiResult<bool>>;

public class UpdateFavoriteSongCommandHandler(ITaikoDbContext context, ILogger<UpdateFavoriteSongCommandHandler> logger)
    : IRequestHandler<UpdateFavoriteSongCommand, ApiResult<bool>>
{
    public async Task<ApiResult<bool>> Handle(UpdateFavoriteSongCommand request, CancellationToken cancellationToken)
    {
        var userDatum = await context.UserData.FindAsync([request.Baid], cancellationToken);
        if (userDatum is null)
        {
            return ApiResult.Failed<bool>("User not found");
        }

        var favoriteSet = new HashSet<uint>(userDatum.FavoriteSongsArray);
        if (request.IsFavorite)
        {
            favoriteSet.Add(request.SongId);
        }
        else
        {
            favoriteSet.Remove(request.SongId);
        }
        

        userDatum.FavoriteSongsArray = favoriteSet.ToList();
        context.Update(userDatum);
        await context.SaveChangesAsync(cancellationToken);
        
        return ApiResult.Succeed(true);
    }
}