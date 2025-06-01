using AutoMapper;
using FinancialApp.Application.DTOs;
using FinancialApp.Domain.Entities;
using FinancialApp.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace FinancialApp.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IAuthRepository _authRepository;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        

        public UserService(
               IAuthRepository authRepository,
               ITokenService tokenService,
               IHttpContextAccessor httpContextAccessor,
               IMapper mapper, ILogger<UserService> logger)
        {
            _authRepository = authRepository;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _logger = logger;
        }
        public Guid GetUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("sub")?.Value;
            if (Guid.TryParse(userIdClaim, out var userId)) return userId;
            throw new Exception("Invalid user token.");
        }

        public async Task<UserResponse> RegisterAsync(UserRegisterRequest request)
        {
            var newUser = await _authRepository.RegisterAsync(request);
            
            _logger.LogInformation("User created successfully");

            var response = _mapper.Map<UserResponse>(newUser);
            var token = await _tokenService.GenerateToken(newUser);

            newUser.CreateAt = DateTime.Now;
            newUser.UpdateAt = DateTime.Now;

            response.AccessToken = token;
            response.CreateAt = newUser.CreateAt;
            response.UpdateAt = newUser.UpdateAt;

            return response;
        }

  

        public async Task<UserResponse> LoginAsync(UserLoginRequest request)
        {
            if (request == null)
            {
                _logger.LogError("Login request is null");
                throw new ArgumentNullException(nameof(request));
            }

            var user = await _authRepository.LoginAsync(request);


            // Generate access token
            var token = await _tokenService.GenerateToken(user); 

            var userResponse = _mapper.Map<ApplicationUser, UserResponse>(user);
            userResponse.AccessToken = token;

            return userResponse;
        }
    }
}
