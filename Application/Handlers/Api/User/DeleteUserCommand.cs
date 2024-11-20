namespace Application.Handlers.Api.User;

public record DeleteUserCommand(uint Baid) : IRequest<ApiResult<bool>>;

public class DeleteUserCommandHandler(ITaikoDbContext context) : IRequestHandler<DeleteUserCommand, ApiResult<bool>>
{
    public async Task<ApiResult<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var userDatum = await context.UserData.FindAsync([request.Baid], cancellationToken);
        if (userDatum == null)
        {
            return ApiResult.Failed<bool>("User not found.");   
        }
        context.UserData.Remove(userDatum);
        await context.SaveChangesAsync(cancellationToken);

        return ApiResult.Succeed(true);
    }
}