using BookingWebApi.Application.User.Query;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Application.User.Validator
{
    public class UserUpdateQueryValidator : AbstractValidator<UserUpdateQuery>
    {
        public UserUpdateQueryValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Write valid email");
            RuleFor(x => x.UserName)
                .MinimumLength(2).WithMessage("Minimum length is 2");
        }
    }
}
