using AutoMapper;
using Azure.Core;
using FinancialApp.Application.DTOs;
using FinancialApp.Domain.Entities;
using FinancialApp.Infrastructure.Persistence.Data;
using FinancialApp.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace FinancialApp.Application.Services
{
    public class StatementService : IStatementService
    {
        private readonly IStatementRepository _statementRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        public StatementService(IMapper mapper, 
            IStatementRepository statementRepository,
            IUserService userService)
        {
            _mapper = mapper;
            _statementRepository = statementRepository;
            _userService = userService;
        }
        public async Task<CreditCardStatementDto> createStatement(StatementDto statementdto)
        {
            var userId = _userService.GetUserId();
            var statement = _mapper.Map<Statement>(statementdto);
            await _statementRepository.create(userId, statement);

            return _mapper.Map<CreditCardStatementDto>(statement);
        }
    }
}
