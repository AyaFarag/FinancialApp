using AutoMapper;
using FinancialApp.Application.DTOs;
using FinancialApp.Domain.Entities;

namespace FinancialApp.Application.Automapper
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<Transaction, TransactionDto>()
                .ForMember(dest => dest.AmountDue, opt => opt.MapFrom(src => src.Amount)) // Direct mapping
                .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.Date));

           
            CreateMap<TransactionDto, Transaction>()
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.AmountDue))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.DueDate));
        }
    }
}
