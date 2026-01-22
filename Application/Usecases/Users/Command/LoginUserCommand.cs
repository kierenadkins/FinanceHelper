using FinanceHelper.Application.Services.Encryption;
using FinanceHelper.Application.Services.Session;
using FinanceHelper.Application.Services.User;
using FinanceHelper.Application.Tools;
using FinanceHelper.Domain.Objects.Base;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace FinanceHelper.Application.Usecases.Users.Command
{
    public class LoginUserQuery : IRequest<BaseResult>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, BaseResult>
    {
        private readonly IHashingService _hashingService;
        private readonly IUserAccountService _userAccountService;
        private readonly ISessionManagerService _sessionManager;

        public LoginUserQueryHandler(
            IHashingService hashingService,
            IUserAccountService userAccountService,
            ISessionManagerService sessionManager)
        {
            _hashingService = hashingService;
            _userAccountService = userAccountService;
            _sessionManager = sessionManager;
        }

        public async Task<BaseResult> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userAccountService.GetUserByEmail(request.Email);

            if (user == null)
                return new BaseResult("Email or Password is incorrect");

            var isPasswordCorrect = _hashingService.VerifyHash(request.Password, user.Password);

            if (!isPasswordCorrect)
            {
                return new BaseResult("Email or Password is incorrect");
            }

            _sessionManager.Set(user.Id, "CurrentUser");

            return new BaseResult();
        }

        public class LoginUserQueryValidator : AbstractValidator<LoginUserQuery>
        {
            public LoginUserQueryValidator()
            {
                RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("Email is required.")
                    .MaximumLength(100).WithMessage("Email must not exceed 100 characters.")
                    .EmailAddress().WithMessage("Please enter a valid email address.");

                RuleFor(x => x.Password)
                    .NotEmpty().WithMessage("Password is required.")
                    .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                    .MaximumLength(50).WithMessage("Password must not exceed 50 characters.")
                    .Must(StringTools.IsValidPassword)
                    .WithMessage("Please enter a valid password.");
            }
        }
    }
}
