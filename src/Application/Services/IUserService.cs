using FinancialApp.Application.DTOs;

namespace FinancialApp.Application.Services
{
    public interface IUserService
    {
        public Guid GetUserId();
        Task<UserResponse> RegisterAsync(UserRegisterRequest request);
        Task<UserResponse> LoginAsync(UserLoginRequest request);
 
    }
}
