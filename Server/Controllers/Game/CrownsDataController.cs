using Application.Utils;

namespace Server.Controllers.Game;

[ApiController]
public class CrownsDataController : BaseController<CrownsDataController>
{
    [HttpPost("/v12r08_ww/chassis/crownsdata_oqgqy90s.php")]
    [Produces("application/protobuf")]
    public async Task<IActionResult> CrownsData([FromBody] CrownsDataRequest request)
    {
        Logger.LogInformation("CrownsData request : {Request}", request.Stringify());

        var crownData = await Mediator.Send(new GetCrownDataQuery(request.Baid));

        var response = new CrownsDataResponse
        {
            Result = 1,
            CrownFlg = crownData.CrownFlg,
            DondafulCrownFlg = crownData.DondafulCrownFlg
        };

        return Ok(response);
    }
    
    [HttpPost("/v12r00_cn/chassis/crownsdata.php")]
    [Produces("application/protobuf")]
    public async Task<IActionResult> CrownsData3209([FromBody] Server.Models.v3209.CrownsDataRequest request)
    {
        Logger.LogInformation("CrownsData request : {Request}", request.Stringify());

        var crownData = await Mediator.Send(new GetCrownDataQuery((uint)request.Baid));
        
        var response = new Server.Models.v3209.CrownsDataResponse
        {
            Result = 1,
            CrownFlg = crownData.CrownFlg,
            DondafulCrownFlg = crownData.DondafulCrownFlg
        };
        
        return Ok(response);
    }
}