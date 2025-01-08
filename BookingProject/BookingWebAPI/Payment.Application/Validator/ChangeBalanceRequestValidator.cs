using FluentValidation;
using Payment.Application.EntityDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Validator
{
    public class ChangeBalanceRequestValidator: AbstractValidator<ChangeBalanceRequest>
    {
        public ChangeBalanceRequestValidator()
        {
            RuleFor(x => x.AdminId)
                .NotNull().WithMessage("AdminId can`t be null");

            RuleFor(x => x.UserId)
                .NotNull().WithMessage("UserId can`t be null");

            RuleFor(x => x.CurrenctName)
                .NotNull().WithMessage("CurrenctName can`t be null");

            RuleFor(x => x.NewBalance)
                .NotNull().WithMessage("Balance can`t be null");
        }
    }
}
