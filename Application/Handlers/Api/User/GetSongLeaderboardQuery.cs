namespace Application.Handlers.Api.User;

using LeaderBoard = PaginatedResult<SongLeaderboardEntry>;
public record GetSongLeaderboardQuery(uint SongId, Difficulty Difficulty, uint Baid, int Page, int Limit) : IRequest<ApiResult<LeaderBoard>>;

public class GetSongLeaderboardQueryHandler(ITaikoDbContext context, ILogger<GetSongLeaderboardQueryHandler> logger)
    : IRequestHandler<GetSongLeaderboardQuery, ApiResult<LeaderBoard>>
{
    public async Task<ApiResult<LeaderBoard>> Handle(GetSongLeaderboardQuery request, CancellationToken cancellationToken)
    {
        var totalScores = await context.SongBestData
            .Where(x => x.SongId == request.SongId && x.Difficulty == request.Difficulty)
            .CountAsync(cancellationToken);

        var totalPages = (totalScores + request.Limit - 1) / request.Limit;

        var scores = await context.SongBestData
            .Where(x => x.SongId == request.SongId && x.Difficulty == request.Difficulty)
            .Select(x => new
            {
                x.Baid,
                x.BestScore,
                x.BestRate,
                x.BestCrown,
                x.BestScoreRank,
                Rank = context.SongBestData.Count(y => y.SongId == request.SongId && y.Difficulty == request.Difficulty && y.BestScore > x.BestScore) + 1
            })
            .OrderByDescending(x => x.BestScore)
            .ThenByDescending(x => x.BestRate)
            .ThenByDescending(x => x.BestCrown)
            .Skip((request.Page - 1) * request.Limit)
            .Take(request.Limit)
            .ToListAsync(cancellationToken);
        
        var userIds = scores.Select(x => x.Baid).Distinct().ToList();
        var users = await context.UserData
            .Where(x => userIds.Contains(x.Baid))
            .ToDictionaryAsync(x => x.Baid, cancellationToken);

        var leaderboard = scores.Select(score =>
        {
            var user = users.GetValueOrDefault(score.Baid);
            return new SongLeaderboardEntry
            {
                Rank = score.Rank,
                Baid = score.Baid,
                UserName = user?.MyDonName,
                BestScore = score.BestScore,
                BestRate = score.BestRate,
                BestCrown = score.BestCrown,
                BestScoreRank = score.BestScoreRank
            };
        }).ToList();
        
        var userLeaderboardEntry = context.SongBestData
            .Where(x => x.SongId == request.SongId && x.Difficulty == request.Difficulty && x.Baid == request.Baid)
            .Select(x => new SongLeaderboardEntry
            {
                Baid = x.Baid,
                BestScore = x.BestScore,
                BestRate = x.BestRate,
                BestCrown = x.BestCrown,
                BestScoreRank = x.BestScoreRank,
                Rank = context.SongBestData.Count(y => y.SongId == request.SongId && y.Difficulty == request.Difficulty && y.BestScore > x.BestScore) + 1
            })
            .FirstOrDefault();
        /*foreach (var score in scores)
        {
            var user = await context.UserData
                .Where(x => x.Baid == score.Baid)
                .FirstOrDefaultAsync(cancellationToken);

            var rank = await context.SongBestData
                .Where(x => x.SongId == request.SongId && x.Difficulty == request.Difficulty && x.BestScore > score.BestScore)
                .CountAsync(cancellationToken);

            leaderboard.Add(new SongLeaderboardEntry
            {
                Rank = rank + 1,
                Baid = score.Baid,
                UserName = user?.MyDonName,
                BestScore = score.BestScore,
                BestRate = score.BestRate,
                BestCrown = score.BestCrown,
                BestScoreRank = score.BestScoreRank
            });
        }*/

        /*SongLeaderboardEntry? userBestScore = null;
        if (request.Baid != 0)
        {
            var score = await context.SongBestData
                .Where(x => x.SongId == request.SongId && x.Difficulty == request.Difficulty && x.Baid == request.Baid)
                .FirstOrDefaultAsync(cancellationToken);

            if (score != null)
            {
                var user = await context.UserData
                    .Where(x => x.Baid == request.Baid)
                    .FirstOrDefaultAsync(cancellationToken);

                var rank = await context.SongBestData
                    .Where(x => x.SongId == request.SongId && x.Difficulty == request.Difficulty && x.BestScore > score.BestScore)
                    .CountAsync(cancellationToken);

                userBestScore = new SongLeaderboardEntry
                {
                    Rank = rank + 1,
                    Baid = score.Baid,
                    UserName = user?.MyDonName,
                    BestScore = score.BestScore,
                    BestRate = score.BestRate,
                    BestCrown = score.BestCrown,
                    BestScoreRank = score.BestScoreRank
                };
            }
        }*/

        return ApiResult.Succeed(new LeaderBoard
        {
            Data = leaderboard,
            Current = userLeaderboardEntry,
            CurrentPage = request.Page,
            TotalPages = totalPages,
            TotalCount = totalScores
        });
    }
}