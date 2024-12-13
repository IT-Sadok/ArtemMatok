using BookingWebApi.Application.User.Query;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Application.User.Validator
{
    public class UserCustomDataValidator : AbstractValidator<UserCustomData>
    {
        
        public UserCustomDataValidator()
        {
            RuleFor(x => x.Key)
                .NotEmpty().WithMessage("Write something")
                .MinimumLength(2).WithMessage("Minimum length 2");

            RuleFor(x => x.Value)
                .NotEmpty().WithMessage("Write something")
                .MinimumLength(2).WithMessage("Minimum length 2");
        }
    }
}
