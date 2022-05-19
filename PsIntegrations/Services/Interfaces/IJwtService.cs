using Common.Models;

namespace PsIntegrations.Interfaces
{
    public interface IJwtService
    {
        JwtResponse CreateToken();
        bool ValidateToken(JwtResponse token);
    }
}
