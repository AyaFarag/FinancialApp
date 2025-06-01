using AutoMapper;
using FinancialApp.Application.DTOs;
using FinancialApp.Domain.Entities;

namespace FinancialApp.Application.Automapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserResponse>();
            CreateMap<UserRegisterRequest, ApplicationUser>();
        }
    }
}
