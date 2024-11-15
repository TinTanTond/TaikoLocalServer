using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GameDatabase.Context;
using SharedProject.Models;
using SharedProject.Models.Responses;
using Swan.Mapping;
using SharedProject.Utils;
using TaikoLocalServer.Mappers;

namespace TaikoLocalServer.Services;

public class AuthService(TaikoDbContext context, ILogger<AuthService> logger) : IAuthService
{
    public async Task<Card?> GetCardByAccessCode(string accessCode)
    {
        return await context.Cards.FindAsync(accessCode);
    }
    
    public async Task<User?> GetUserByBaid(uint baid)
    {
        var userDatum = await context.UserData.Include(datum => datum.Cards)
            .Where(datum => datum.Baid == baid).FirstOrDefaultAsync();
        if (userDatum == null) return null;
        return new User
        {
            Baid = userDatum.Baid,
            AccessCodes = userDatum.Cards.Select(card => card.AccessCode).ToList(),
            IsAdmin = userDatum.IsAdmin
        };
    }

    public async Task<UsersResponse> GetUsersFromCards(int page = 1, int limit = 12, string? searchTerm = null)
    {
        var users = new List<User>();

        var cardEntries = await context.Cards.ToListAsync();
        var userEntriesQuery = context.UserData.AsQueryable();

        // If a search term is provided, filter the users based on the search term
        if (!string.IsNullOrEmpty(searchTerm))
        {
            var lowerCaseSearchTerm = searchTerm.ToLower();
            userEntriesQuery = userEntriesQuery.Where(user => user.Baid.ToString() == lowerCaseSearchTerm
                                                              || user.MyDonName.ToLower().Contains(lowerCaseSearchTerm)
                                                              || context.Cards.Any(card => card.Baid == user.Baid && card.AccessCode.ToLower().Contains(lowerCaseSearchTerm)));
        }

        // Get the total count of users after applying the search term
        var totalUsers = await userEntriesQuery.CountAsync();

        // Calculate the total pages
        var totalPages = totalUsers / limit;

        // If there is a remainder, add one to the total pages
        if (totalUsers % limit > 0)
        {
            totalPages++;
        }

        var userEntries = await userEntriesQuery
            .OrderBy(user => user.Baid)
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();

        foreach (var user in userEntries)
        {
            List<List<uint>> costumeUnlockData =
                [user.UnlockedKigurumi, user.UnlockedHead, user.UnlockedBody, user.UnlockedFace, user.UnlockedPuchi];

            var unlockedTitle = user.TitleFlgArray
                .ToList();

            for (var i = 0; i < 5; i++)
            {
                if (!costumeUnlockData[i].Contains(0))
                {
                    costumeUnlockData[i].Add(0);
                }
            }

            var userSetting = UserSettingMapper.MapToUserSetting(user);

            users.Add(new User
            {
                Baid = user.Baid,
                AccessCodes = cardEntries.Where(card => card.Baid == user.Baid).Select(card => card.AccessCode).ToList(),
                IsAdmin = user.IsAdmin,
                UserSetting = userSetting
            });
        }
        
        return new UsersResponse
        {
            Users = users,
            Page = page,
            TotalPages = totalPages,
            TotalUsers = totalUsers
        };
    }
    
    public async Task AddCard(Card card)
    {
        context.Add(card);
        await context.SaveChangesAsync();
    }

    public async Task<bool> DeleteCard(string accessCode)
    {
        var card = await context.Cards.FindAsync(accessCode);
        if (card == null) return false;
        context.Cards.Remove(card);
        await context.SaveChangesAsync();
        return true;
    }
    
    public async Task<List<UserCredential>> GetUserCredentialsFromCredentials()
    {
        return await context.Credentials.Select(credential => credential.CopyPropertiesToNew<UserCredential>(null)).ToListAsync();
    }

    public async Task AddCredential(Credential credential)
    {
        context.Add(credential);
        await context.SaveChangesAsync();
    }

    public async Task<bool> DeleteCredential(uint baid)
    {
        var credential = await context.Credentials.FindAsync(baid);

        if (credential is null) return false;

        context.Credentials.Remove(credential);
        await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdatePassword(uint baid, string password, string salt)
    {
        var credential = await context.Credentials.FindAsync(baid);

        if (credential is null) return false;

        credential.Password = password;
        credential.Salt = salt;
        await context.SaveChangesAsync();

        return true;
    }
    
    public async Task<Credential?> GetCredentialByBaid(uint baid)
    {
        return await context.Credentials.FindAsync(baid);
    }
    
    public (uint baid, bool isAdmin)? ExtractTokenInfo(HttpContext httpContext)
    {
        var authHeader = httpContext.Request.Headers.Authorization.FirstOrDefault();
        if (authHeader == null || !authHeader.StartsWith("Bearer "))
        {
            logger.LogWarning("Invalid auth header");
            return null;
        }

        var token = authHeader["Bearer ".Length..].Trim();
        var handler = new JwtSecurityTokenHandler();
        if (!handler.CanReadToken(token))
        {
            logger.LogWarning("Invalid token");
            return null;
        }
        
        var jwtToken = handler.ReadJwtToken(token);
        if (jwtToken.ValidTo < DateTime.UtcNow)
        {
            logger.LogWarning("Token expired");
            return null;
        }
        
        var claimBaid = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        var claimRole = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        if (claimBaid == null || claimRole == null)
        {
            logger.LogWarning("Invalid token claims");
            return null;
        }

        if (!uint.TryParse(claimBaid, out var baid))
        {
            logger.LogWarning("Invalid baid");
            return null;
        }
        var isAdmin = claimRole == "Admin";
        return (baid, isAdmin);
    }
}