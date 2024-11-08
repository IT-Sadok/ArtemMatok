using BookingWebApi.Application.DTOs.AppUserDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Application.Validation.AppUserValid
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Please enter a valid email address.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Passwords must be at least 8 characters.")
                .Matches(@"[A-Z]").WithMessage("Passwords must have at least one uppercase ('A'-'Z').")
                .Matches(@"\d").WithMessage("Passwords must have at least one digit ('0'-'9').")
                .Matches(@"[\W_]").WithMessage("Passwords must have at least one non-alphanumeric character."); // This checks for special characters

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .Must(userName => !string.IsNullOrWhiteSpace(userName?.Trim()))
                .WithMessage("Username cannot be empty or just spaces.")
                .MinimumLength(2).WithMessage("User name must be more than 2 symbols (excluding spaces).");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required.")
                .Must(role => role == "Host" || role == "User")
                .WithMessage("Role must be either 'Host' or 'User'.");
        }
    }
}
