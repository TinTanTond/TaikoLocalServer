using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Server.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class GameDataController(IGameDataService gameDataService) : BaseController<UsersController>
{
    [HttpGet("MusicDetails")] 
    [Authorize(Policy = "AuthConditional")]
    public IActionResult GetMusicDetails()
    {
        return Ok(gameDataService.GetMusicDetailDictionary());
    }
    
    [HttpGet("Costumes")]
    [Authorize(Policy = "AuthConditional")]
    public IActionResult GetCostumes()
    {
        return Ok(gameDataService.GetCostumeList());
    }
    
    [HttpGet("Titles")]
    [Authorize(Policy = "AuthConditional")]
    public IActionResult GetTitles()
    {
        return Ok(gameDataService.GetTitleDictionary());
    }
    
    [HttpGet("LockedCostumes")]
    [Authorize(Policy = "AuthConditional")]
    public IActionResult GetLockedCostumes()
    {
        return Ok(gameDataService.GetLockedCostumeDataDictionary());
    }
    
    [HttpGet("LockedTitles")]
    [Authorize(Policy = "AuthConditional")]
    public IActionResult GetLockedTitles()
    {
        return Ok(gameDataService.GetLockedTitleDataDictionary());
    }
}