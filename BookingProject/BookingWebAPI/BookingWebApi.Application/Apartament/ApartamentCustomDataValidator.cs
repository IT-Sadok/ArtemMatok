using BookingWebApi.Application.Apartament.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Application.Apartament
{
    public class ApartamentCustomDataValidator : AbstractValidator<ApartamentCustomDataDto>
    {
        public ApartamentCustomDataValidator()
        {
            RuleFor(x => x.Key)
                .NotEmpty().WithMessage("The key is required.")
                .MaximumLength(100).WithMessage("The key cannot be longer than 100 characters.");

            RuleFor(x => x.Value)
                .NotEmpty().WithMessage("Value is required")
                .NotNull().WithMessage("The value cannot be null.");
        }
    }
}
