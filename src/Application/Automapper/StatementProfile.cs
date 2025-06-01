using AutoMapper;
using FinancialApp.Application.DTOs;
using FinancialApp.Domain.Entities;

namespace FinancialApp.Application.Automapper
{
    public class StatementProfile : Profile
    {
        public StatementProfile()
        {
            CreateMap<Statement, CreditCardStatementDto>()
                      .ForMember(dest => dest.AmountDue, opt => opt.MapFrom(src => Math.Round(src.AmountDue, 2)));

            CreateMap<CreditCardStatementDto, Statement>();

            CreateMap<Statement, StatementDto>()
                        .ForMember(dest => dest.AmountDue, opt => opt.MapFrom(src => Math.Round(src.AmountDue, 2)));

            CreateMap<StatementDto, Statement>();
        }
    }
}
