using Contracts.DTOs.Payment;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Validators
{
    public class BalanceRequestDtoValidator : AbstractValidator<BalanceRequestDto>
    {
        public BalanceRequestDtoValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User id is required");
            RuleFor(x => x.CurrencyName)
                .NotEmpty().WithMessage("Currency name is required")
                .MinimumLength(2).WithMessage("Minimum length is 2");
            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("Price id is required")
                .GreaterThan(1).WithMessage("Price shoud be grater than 0");
        }
    }
}

