namespace Application.Interfaces;

public interface IJwtTokenService
{
    public string GenerateToken(uint baid, bool isAdmin);
}