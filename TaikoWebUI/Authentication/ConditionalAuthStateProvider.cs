using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Options;
using TaikoWebUI.Settings;

namespace TaikoWebUI.Authentication;

public class ConditionalAuthStateProvider(IOptions<WebUiSettings> settings, ILocalStorageService localStorage, LoginService loginService) 
    : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var isAuthRequired = settings.Value.LoginRequired;

        if (isAuthRequired)
        {
            var token = await localStorage.GetItemAsync<string>("authToken");

            if (string.IsNullOrEmpty(token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var claims = ParseClaimsFromJwt(token);
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));

            return new AuthenticationState(user);
        }
        // If authentication is not required, return an authenticated user
        var identity = new ClaimsIdentity([
            new Claim(ClaimTypes.Name, "1"),
            new Claim(ClaimTypes.Role, "Admin")
        ], "No Auth");

        var noAuthUser = new ClaimsPrincipal(identity);
        return new AuthenticationState(noAuthUser);
    }
    
    
    public async Task LoginAsync(string username, string password)
    {
        var token = await loginService.Login(username, password);

        if (!string.IsNullOrEmpty(token))
        {
            await localStorage.SetItemAsync("authToken", token);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }

    public async Task LogoutAsync()
    {
        await localStorage.RemoveItemAsync("authToken");
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
    
    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var handler = new JwtSecurityTokenHandler();
        if (!handler.CanReadToken(jwt))
        {
            return [];
        }
        var token = handler.ReadJwtToken(jwt);
        return token.Claims;
    }
}
