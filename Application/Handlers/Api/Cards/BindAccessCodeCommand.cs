namespace Application.Handlers.Api.Cards;

public record BindAccessCodeCommand(string AccessCode, uint Baid) : IRequest<ApiResult<bool>>;

public class BindAccessCodeCommandHandler(ITaikoDbContext context, ILogger<BindAccessCodeCommandHandler> logger)
    : IRequestHandler<BindAccessCodeCommand, ApiResult<bool>>
{
    public async Task<ApiResult<bool>> Handle(BindAccessCodeCommand request, CancellationToken cancellationToken)
    {
        var existingCard = await context.Cards.FirstOrDefaultAsync(c => c.AccessCode == request.AccessCode, cancellationToken);
        if (existingCard != null)
        {
            return ApiResult.Failed<bool>("Access code already exists");
        }

        var newCard = new Card
        {
            Baid = request.Baid,
            AccessCode = request.AccessCode
        };

        await context.Cards.AddAsync(newCard, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return ApiResult.Succeed(true);
    }
}