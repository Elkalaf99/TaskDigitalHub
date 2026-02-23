using System.Security.Claims;

namespace TaskDigitalhub.Application.Common.Interfaces;

public interface IJwtService
{
    string GenerateToken(int userId, string userName, string email, string role);
}
