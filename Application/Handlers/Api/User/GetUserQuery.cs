namespace Application.Handlers.Api.User;

public record GetUserQuery(uint Baid) : IRequest<ApiResult<Domain.Models.User>>;

public class GetUserQueryHandler(ITaikoDbContext context) : IRequestHandler<GetUserQuery, ApiResult<Domain.Models.User>>
{
    public async Task<ApiResult<Domain.Models.User>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var userDatum = await context.UserData.Include(datum => datum.Cards)
            .Where(datum => datum.Baid == request.Baid)
            .FirstOrDefaultAsync(cancellationToken);

        if (userDatum == null)
        {
            return ApiResult.Failed<Domain.Models.User>("User not found.");
        }
        
        return ApiResult.Succeed(new Domain.Models.User
        {
            Baid = userDatum.Baid,
            AccessCodes = userDatum.Cards.Select(card => card.AccessCode).ToList(),
            IsAdmin = userDatum.IsAdmin
        });
    }
}