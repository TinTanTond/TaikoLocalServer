using System.Text;
using OtpNet;

namespace Application.Utils;

public class TotpUtils
{
    public static Totp MakeTotp(uint baid)
    {
        var secretKey = (baid * 765 + 2023).ToString();
        var base32String = Base32Encoding.ToString(Encoding.UTF8.GetBytes(secretKey));
        var base32Bytes = Base32Encoding.ToBytes(base32String);
        return new Totp(base32Bytes, step: 999999999);
    }
    
    public static bool VerifyOtp(string otp, uint baid)
    {
        var totp = MakeTotp(baid);
        return totp.VerifyTotp(otp, out _);
    }
}