using EventFlow.API.Domain;
using System.Security.Claims;

namespace EventFlow.API.Interfaces.Services
{
    public interface IJwtService
    {
        string generateToken(User user);
        ClaimsPrincipal validateToken(string token);
    }
}
