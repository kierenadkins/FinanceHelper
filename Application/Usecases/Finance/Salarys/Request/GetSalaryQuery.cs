using Application.Domain.Finance;
using Application.Services.Finance;
using Application.Services.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Usecases.Finance.Salarys.Request
{
    public class GetSalaryQuery : IRequest<Salary>
    {
    }

    public class GetSalaryQueryHandler : IRequestHandler<GetSalaryQuery, Salary>
    {
        private readonly ISalaryService _salaryService;
        private readonly IUserAccountService _userAccountService;

        public GetSalaryQueryHandler(ISalaryService salaryService, IUserAccountService userAccountService)
        {
            _userAccountService = userAccountService;
            _salaryService = salaryService;
        }

        public async Task<Salary?> Handle(GetSalaryQuery request, CancellationToken cancellationToken)
        {
            var userId = _userAccountService.GetCurrent();

            if(userId == 0)
            {
                return null;
            }

            return await _salaryService.GetAllByUserIdCacheAsync(userId);
        }
    }
}
