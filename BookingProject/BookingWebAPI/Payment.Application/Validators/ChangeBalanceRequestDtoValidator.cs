using FluentValidation;
using Payment.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Validators
{
    public class ChangeBalanceRequestDtoValidator: AbstractValidator<ChangeBalanceRequestDto>
    {
        public ChangeBalanceRequestDtoValidator()
        {
            RuleFor(x => x.UserId)
                .NotNull().WithMessage("UserId can`t be null");

            RuleFor(x => x.CurrencyName)
                .NotNull().WithMessage("Currency name can`t be null")
                .NotEmpty().WithMessage("Currency name can`t be empty");

            RuleFor(x => x.Amount)
                .NotNull().WithMessage("Amount can`t be null")
                .GreaterThan(0).WithMessage("Amount can`t be less than 0");
        }
    }
}
