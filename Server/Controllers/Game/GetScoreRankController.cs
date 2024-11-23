using Application.Utils;
using Microsoft.Extensions.Options;

namespace Server.Controllers.Game;

[ApiController]
public class GetScoreRankController : BaseController<GetScoreRankController>
{
    [HttpPost("/v12r08_ww/chassis/getscorerank_1c8l7y61.php")]
    [Produces("application/protobuf")]
    public async Task<IActionResult> GetScoreRank([FromBody] GetScoreRankRequest request)
    {
        Logger.LogInformation("GetScoreRank request : {Request}", request.Stringify());

        var scoreRankData = await Mediator.Send(new GetScoreRankQuery(request.Baid));
        var response = new GetScoreRankResponse
        {
            Result = 1,
            IkiScoreRankFlg = scoreRankData.IkiScoreRankFlg,
            KiwamiScoreRankFlg = scoreRankData.KiwamiScoreRankFlg,
            MiyabiScoreRankFlg = scoreRankData.MiyabiScoreRankFlg
        };

        return Ok(response);
    }
    
    [HttpPost("/v12r00_cn/chassis/getscorerank.php")]
    [Produces("application/protobuf")]
    public async Task<IActionResult> GetScoreRank3209([FromBody] Server.Models.v3209.GetScoreRankRequest request)
    {
        Logger.LogInformation("GetScoreRank request : {Request}", request.Stringify());
       
        var scoreRankData = await Mediator.Send(new GetScoreRankQuery((uint)request.Baid));
        var response = new Server.Models.v3209.GetScoreRankResponse
        {
            Result = 1,
            IkiScoreRankFlg = scoreRankData.IkiScoreRankFlg,
            KiwamiScoreRankFlg = scoreRankData.KiwamiScoreRankFlg,
            MiyabiScoreRankFlg = scoreRankData.MiyabiScoreRankFlg
        };

        return Ok(response);
    }
}