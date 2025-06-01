using FinancialApp.Domain.Entities;

namespace FinancialApp.Application.Services
{
    public interface ITokenService
    {
        Task<string> GenerateToken(ApplicationUser user);
        string GenerateRefreshToken();
    }
}
