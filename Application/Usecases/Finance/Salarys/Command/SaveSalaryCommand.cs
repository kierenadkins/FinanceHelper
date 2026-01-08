using Application.Domain.Base;
using Application.Domain.Finance;
using Application.Services.Finance;
using Application.Services.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Usecases.Finance.Salarys.Command
{
    public class SaveSalaryCommand : IRequest<BaseResult>
    {
        public Salary Salary { get; set; }
    }

    public class SaveSalaryCommandHandler : IRequestHandler<SaveSalaryCommand, BaseResult>
    {
        private readonly ISalaryService _salaryService;
        private readonly IUserAccountService _userAccountService;

        public SaveSalaryCommandHandler(ISalaryService salaryService, IUserAccountService userAccountService)
        {
            _salaryService = salaryService;
            _userAccountService = userAccountService;
        }

        public async Task<BaseResult> Handle(SaveSalaryCommand request, CancellationToken cancellationToken)
        {
            var result = new BaseResult();

            var userId = _userAccountService.GetCurrent();

            if (userId == 0)
            {
                result.AddError("User not authenticated.");
                return result;
            }
            request.Salary.UserId = userId;

            await _salaryService.AddAsync(request.Salary);
            return result;
        }
    }
}
