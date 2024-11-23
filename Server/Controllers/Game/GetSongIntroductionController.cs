namespace Server.Controllers.Game;

[ApiController]
public class GetSongIntroductionController : BaseController<GetSongIntroductionController>
{
    [HttpPost("/v12r08_ww/chassis/getsongintroduction_66blw6is.php")]
    [Produces("application/protobuf")]
    public async Task<IActionResult> GetSongIntroduction([FromBody] GetSongIntroductionRequest request)
    {
        Logger.LogInformation("GetSongIntroduction request : {Request}", request.Stringify());

        var commonResponse = await Mediator.Send(new GetSongIntroductionQuery(request.SetIds));
        var response = SongIntroductionDataMappers.MapTo3906(commonResponse);
        
        return Ok(response);
    }
    
    [HttpPost("/v12r00_cn/chassis/getsongintroduction.php")]
    [Produces("application/protobuf")]
    public async Task<IActionResult> GetSongIntroduction3209([FromBody] Server.Models.v3209.GetSongIntroductionRequest request)
    {
        Logger.LogInformation("GetSongIntroduction request : {Request}", request.Stringify());

        var commonResponse = await Mediator.Send(new GetSongIntroductionQuery(request.SetIds));
        var response = SongIntroductionDataMappers.MapTo3209(commonResponse);
        
        return Ok(response);
    }
}