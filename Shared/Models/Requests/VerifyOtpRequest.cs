namespace Shared.Models.Requests;

public class VerifyOtpRequest
{
    public string Otp { get; set; } = "";
    
    public uint Baid { get; set; }
}