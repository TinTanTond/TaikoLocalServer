using System.Text.Json;

namespace TaikoWebUI.Services;

public class LoginService(HttpClient httpClient)
{
    public async Task<string?> Login(string accessCode, string password)
    {
        var response = await httpClient.PostAsJsonAsync("/api/Login", 
            new LoginRequest
            {
                AccessCode = accessCode, Password = password
            });
        
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<string>(responseContent, options);
            return result;
        }
        
        return string.Empty;
    }
}