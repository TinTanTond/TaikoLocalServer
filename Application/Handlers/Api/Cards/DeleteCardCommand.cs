namespace Application.Handlers.Api.Cards;

public record DeleteCardCommand(string AccessCode, uint Baid, bool IsAdmin) : IRequest<ApiResult<bool>>;

public class DeleteCardCommandHandler(ITaikoDbContext context, ILogger<DeleteCardCommandHandler> logger)
    : IRequestHandler<DeleteCardCommand, ApiResult<bool>>
{
    public async Task<ApiResult<bool>> Handle(DeleteCardCommand request, CancellationToken cancellationToken)
    {
        var card = await context.Cards.FirstOrDefaultAsync(c => c.AccessCode == request.AccessCode, cancellationToken);
        if (card == null)
        {
            return ApiResult.Failed<bool>("Card not found");
        }
        
        if (card.Baid != request.Baid && !request.IsAdmin)
        {
            return ApiResult.Failed<bool>("Unauthorized to delete card");
        }

        context.Cards.Remove(card);
        await context.SaveChangesAsync(cancellationToken);
        return ApiResult.Succeed(true);
    }
}