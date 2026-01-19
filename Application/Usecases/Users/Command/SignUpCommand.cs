using FinanceHelper.Application.Services.Encryption;
using FinanceHelper.Application.Services.User;
using FinanceHelper.Application.Validators;
using FinanceHelper.Domain.Objects.Base;
using FinanceHelper.Domain.Objects.Users;
using MediatR;

namespace FinanceHelper.Application.Usecases.Users.Command
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

            var user = await _userAccountService.AddAsync(request.User);

            if(user.Id == 0)
            {
                return new BaseResult("Error occurred while creating the user account");
            }

            return result;
        }
    }
}
