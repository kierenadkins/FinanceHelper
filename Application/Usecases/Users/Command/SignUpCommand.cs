using FinanceHelper.Application.Services.Encryption;
using FinanceHelper.Application.Services.User;
using FinanceHelper.Application.Tools;
using FinanceHelper.Application.Validators;
using FinanceHelper.Domain.Objects.Base;
using FinanceHelper.Domain.Objects.Users;
using FluentValidation;
using MediatR;

namespace FinanceHelper.Application.Usecases.Users.Command
{
    public class SignupCommand : IRequest<BaseResult>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
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
            if (await _userAccountService.GetUserByEmail(request.EmailAddress) != null)
                return new BaseResult("A user with this email already exists.");

            var user = new UserAccount { FirstName = request.FirstName, LastName = request.LastName, EmailAddress = request.EmailAddress, Password = _hashingService.HashPassword(request.Password) };
            await _userAccountService.AddAsync(user);

            return new BaseResult();
        }

        public class SignupCommandValidator : AbstractValidator<SignupCommand>
        {
            public SignupCommandValidator()
            {
                RuleFor(x => x.FirstName)
                    .NotEmpty().WithMessage("First name is required.")
                    .MinimumLength(2).WithMessage("First name must be at least 2 characters long.")
                    .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");


                RuleFor(x => x.LastName)
                    .NotEmpty().WithMessage("Last name is required.")
                    .MinimumLength(2).WithMessage("Last name must be at least 2 characters long.")
                    .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.");


                RuleFor(x => x.EmailAddress)
                    .NotEmpty().WithMessage("Email address is required.")
                    .MaximumLength(100).WithMessage("Email address must not exceed 100 characters.")
                    .MinimumLength(5).WithMessage("Email address must be at least 5 characters long.")
                    .EmailAddress().WithMessage("Please enter a valid email address.");


                RuleFor(x => x.Password)
                    .NotEmpty().WithMessage("Password is required.")
                    .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                    .MaximumLength(50).WithMessage("Password must not exceed 50 characters.")
                    .Must(StringTools.IsValidPassword).WithMessage("Please enter a valid password.");

            }
        }
    }
}
