using Application.Mappers;

namespace Application.Handlers.Api.User;

using Users = PaginatedResult<Domain.Models.User>;

public record GetUsersQuery(int Page, int Limit, string? SearchTerm) : IRequest<ApiResult<Users>>;

public class GetUsersQueryHandler(ITaikoDbContext context) : IRequestHandler<GetUsersQuery, ApiResult<Users>>
{
    public async Task<ApiResult<Users>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = new List<Domain.Models.User>();

        var cardEntries = await context.Cards.ToListAsync(cancellationToken);
        var userEntriesQuery = context.UserData.AsQueryable();

        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            var lowerCaseSearchTerm = request.SearchTerm.ToLower();
            userEntriesQuery = userEntriesQuery.Where(user => 
                user.Baid.ToString() == lowerCaseSearchTerm || 
                user.MyDonName.Contains(lowerCaseSearchTerm, StringComparison.CurrentCultureIgnoreCase) || 
                context.Cards.Any(card => card.Baid == user.Baid && 
                                          card.AccessCode.Contains(lowerCaseSearchTerm, StringComparison.CurrentCultureIgnoreCase)));
        }

        var totalUsers = await userEntriesQuery.CountAsync(cancellationToken);
        var totalPages = (totalUsers + request.Limit - 1) / request.Limit;

        var userEntries = await userEntriesQuery
            .OrderBy(user => user.Baid)
            .Skip((request.Page - 1) * request.Limit)
            .Take(request.Limit)
            .ToListAsync(cancellationToken);

        foreach (var user in userEntries)
        {
            var userSetting = UserSettingMapper.MapToUserSetting(user);

            users.Add(new Domain.Models.User
            {
                Baid = user.Baid,
                AccessCodes = cardEntries.Where(card => card.Baid == user.Baid).Select(card => card.AccessCode).ToList(),
                IsAdmin = user.IsAdmin,
                UserSetting = userSetting
            });
        }

        return ApiResult.Succeed( new Users
        {
            Data = users,
            CurrentPage = request.Page,
            TotalPages = totalPages,
            TotalCount = totalUsers
        });
    }
}