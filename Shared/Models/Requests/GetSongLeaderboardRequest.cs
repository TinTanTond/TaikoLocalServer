using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Shared.Models.Requests;

public class GetSongLeaderboardRequest
{
    public uint SongId { get; set; }
    
    [EnumDataType(typeof(Difficulty), ErrorMessage = "Difficulty must be a valid value.")]
    public Difficulty Difficulty { get; set; }
    
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Page number cannot be less than 1.")]
    public int Page { get; set; } = 1;
    
    [Range(1, 200, ErrorMessage = "Limit cannot be greater than 200.")]
    public int Limit { get; set; } = 10;
}