using Application.Models.Game;

namespace Application.Handlers.Game;

public record AddTokenCountCommand(CommonAddTokenCountRequest Request) : IRequest;

public class AddTokenCountCommandHandler(ITaikoDbContext context, ILogger<AddTokenCountCommandHandler> logger)
    : IRequestHandler<AddTokenCountCommand>
{
    private readonly ILogger<AddTokenCountCommandHandler> logger = logger;

    public async Task Handle(AddTokenCountCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var user = await context.UserData
            .Include(userDatum => userDatum.Tokens)
            .FirstOrDefaultAsync(datum => datum.Baid == request.Baid, cancellationToken);
        user.ThrowIfNull($"User with baid {request.Baid} does not exist!");

        foreach (var addTokenCountData in request.AryAddTokenCountDatas)
        {
            var tokenId = addTokenCountData.TokenId;
            var addTokenCount = addTokenCountData.AddTokenCount;
            var token = user.Tokens.FirstOrDefault(t => t.Id == tokenId);
            if (token is not null)
            {
                token.Count += addTokenCount;
            }
            else
            {
                user.Tokens.Add(new Token
                {
                    Baid = user.Baid,
                    Id = (int)tokenId,
                    Count = addTokenCount
                });
            }
        }

        context.Update(user);
        await context.SaveChangesAsync(cancellationToken);
    }
}