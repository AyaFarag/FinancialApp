using FinancialApp.Application.DTOs;
using FinancialApp.Domain.Entities;

namespace FinancialApp.Infrastructure.Repositories
{
    public interface IAuthRepository
    {
        Task<ApplicationUser> RegisterAsync(UserRegisterRequest request);
        Task<ApplicationUser> LoginAsync(UserLoginRequest request);
    }
}
