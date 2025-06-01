using AutoMapper;
using FinancialApp.Application.DTOs;
using FinancialApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace FinancialApp.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public AuthRepository(UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<ApplicationUser> LoginAsync(UserLoginRequest request)
        {

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                throw new Exception("Invalid email or password");
            }
            return user;
        }

        public async Task<ApplicationUser> RegisterAsync(UserRegisterRequest request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {

                throw new Exception("Email already exists");
            }
            var newUser = _mapper.Map<ApplicationUser>(request);

            newUser.UserName = GenerateUserName(request.FirstName, request.LastName);

            var result = await _userManager.CreateAsync(newUser, request.Password);
            return newUser;
        }

        private string GenerateUserName(string firstName, string lastName)
        {
            var baseUsername = $"{firstName}{lastName}".ToLower();

            // Check if the username already exists
            var username = baseUsername;
            var count = 1;
            while (_userManager.Users.Any(u => u.UserName == username))
            {
                username = $"{baseUsername}{count}";
                count++;
            }
            return username;
        }
    }
}
