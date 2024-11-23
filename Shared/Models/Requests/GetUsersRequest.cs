using System.ComponentModel.DataAnnotations;

namespace Shared.Models.Requests;

public class GetUsersRequest
{
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Page number cannot be less than 1.")]
    public int Page { get; set; } = 1;
    
    [Range(1, 200, ErrorMessage = "Limit cannot be greater than 200.")]
    public int Limit { get; set; } = 10;
    
    public string? SearchTerm { get; set; }
}