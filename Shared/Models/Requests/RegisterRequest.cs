using System.ComponentModel.DataAnnotations;

namespace Shared.Models.Requests;

public class RegisterRequest
{
    public string AccessCode { get; set; } = string.Empty;
    
    [Required]
    [StringLength(32, MinimumLength = 1, ErrorMessage = "Password must be between 1 and 32 characters.")]
    public string Password { get; set; } = string.Empty;
    public bool RegisterWithLastPlayTime { get; set; }
    public DateTime LastPlayDateTime { get; set; }
    public string InviteCode { get; set; } = string.Empty;
}