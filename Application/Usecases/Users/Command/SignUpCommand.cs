using Application.Domain.Base;
using Application.Domain.Users;
using Application.Services.Encryption;
using Application.Services.User;
using Application.Validators;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Usecases.Users.Command
{
    public class SignupCommand : IRequest<BaseResult>
    {
        public UserAccount User { get; set; }
    }

    public class SignupCommandHandler : IRequestHandler<SignupCommand, BaseResult>
    {
        private readonly IHashingService _hashingService;
        private readonly IUserAccountService _userAccountService;

        public SignupCommandHandler(
            IHashingService hashingService,
            IUserAccountService userAccountService)
        {
            _hashingService = hashingService;
            _userAccountService = userAccountService;
        }

        public async Task<BaseResult> Handle(SignupCommand request, CancellationToken cancellationToken)
        {
            var result = new BaseResult();
            var validator = new UserAccountValidator();
            var validationResult = await validator.ValidateAsync(request.User, cancellationToken);

            if (!validationResult.IsValid)
            {
                result.AddErrors(validationResult.Errors.Select(e => e.ErrorMessage));
                return result;
            }

            var existingUser = await _userAccountService.GetUserByEmail(request.User.EmailAddress);

            if (existingUser != null)
            {
                return new BaseResult("An User with this email, already exists");
            }

            request.User.Password = _hashingService.HashPassword(request.User.Password);

            await _userAccountService.Add(request.User);

            return result;
        }
    }
}
