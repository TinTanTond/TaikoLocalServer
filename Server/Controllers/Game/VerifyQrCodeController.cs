namespace Server.Controllers.Game;

[ApiController]
public class VerifyQrCodeController : BaseController<VerifyQrCodeController>
{
    [HttpPost("/v12r08_ww/chassis/verifyqrcode_ku5ra5q7.php")]
    [Produces("application/protobuf")]
    public async Task<IActionResult> VerifyQrCode([FromBody] VerifyQrcodeRequest request)
    {
        Logger.LogInformation("VerifyQrCode request : {Request}", request.Stringify());

        var commonResponse = await Mediator.Send(new VerifyQrQuery(request.QrcodeSerial));
        var response = new VerifyQrcodeResponse
        {
            Result = commonResponse.IsQrValid ? 1u : 51u,
            QrcodeId = commonResponse.QrCodeId
        };

        return Ok(response);
    }
    
    [HttpPost("/v12r00_cn/chassis/verifyqrcode.php")]
    [Produces("application/protobuf")]
    public async Task<IActionResult> VerifyQrCode3209([FromBody] Server.Models.v3209.VerifyQrcodeRequest request)
    {
        Logger.LogInformation("VerifyQrCode request : {Request}", request.Stringify());
        
        var commonResponse = await Mediator.Send(new VerifyQrQuery(request.QrcodeSerial));
        var response = new Models.v3209.VerifyQrcodeResponse
        {
            Result = commonResponse.IsQrValid ? 1u : 51u,
            QrcodeId = commonResponse.QrCodeId
        };

        return Ok(response);
        
    }
}