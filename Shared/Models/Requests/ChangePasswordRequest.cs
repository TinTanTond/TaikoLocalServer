using System.ComponentModel.DataAnnotations;

namespace Shared.Models.Requests;

public class ChangePasswordRequest
{
    public uint Baid { get; set; }
    
    public string OldPassword { get; set; } = string.Empty;
    
    [Required]
    [StringLength(32, MinimumLength = 1, ErrorMessage = "Password must be between 1 and 32 characters.")]
    public string NewPassword { get; set; } = string.Empty;
}