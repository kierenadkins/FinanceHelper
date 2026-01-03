using Application.Domain.Users;
using Application.Tools;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Validators
{
    public class UserAccountValidator : AbstractValidator<UserAccount>
    {
        public UserAccountValidator()
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
