using FinanceHelper.Application.Interfaces;
using FinanceHelper.Application.Services.Encryption;
using FinanceHelper.Application.Services.User;
using FinanceHelper.Application.Tools;
using FinanceHelper.Domain.Objects.Base;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceHelper.Application.Usecases.Users.Command
{
    public class LoginQuery : IRequest<BaseResult>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginQueryHandler : IRequestHandler<LoginQuery, BaseResult>
    {
        private readonly IHashingService _hashingService;
        private readonly IEncryptionService _encryptionService;
        private readonly IUserAccountService _userAccountService;
        private readonly ISessionManagerService _sessionManager;

        public LoginQueryHandler(
            IHashingService hashingService,
            IEncryptionService encryptionService,
            IUserAccountService userAccountService,
            ISessionManagerService sessionManager)
        {
            _hashingService = hashingService;
            _encryptionService = encryptionService;
            _userAccountService = userAccountService;
            _sessionManager = sessionManager;
        }

        public async Task<BaseResult> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            if (!StringTools.IsValidEmail(request.Email)) { return new BaseResult("Please ensure you use a valid email address"); }
            if (!StringTools.IsValidPassword(request.Password)) { return new BaseResult("Please ensure you use a valid password"); }

            var user = await _userAccountService.GetUserByEmail(request.Email);

            if (user == null)
            {
                return new BaseResult("Email or Password is incorrect");
            }

            var isPasswordCorrect = _hashingService.VerifyHash(request.Password, user.Password);

            if (!isPasswordCorrect)
            {
                return new BaseResult("Email or Password is incorrect");
            }

            _sessionManager.Set(user.Id, "CurrentUser");

            return new BaseResult();
        }
    }
}
