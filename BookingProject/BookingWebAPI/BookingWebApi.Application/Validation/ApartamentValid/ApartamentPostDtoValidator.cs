using BookingWebApi.Application.DTOs.ApartamentDTOs;
using BookingWebApi.Application.DTOs.AppUserDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Application.Validation.ApartamentValid
{
    public class ApartamentPostDtoValidator : AbstractValidator<ApartamentPostDto>
    {
        public ApartamentPostDtoValidator()
        {
            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(100).WithMessage("Address must not exceed 100 characters.");

            RuleFor(x => x.Area)
                .GreaterThan(0).WithMessage("Area must be greater than 0.");

            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90.");

            RuleFor(x => x.Longtitude)
                .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180.");

            RuleFor(x => x.Bedrooms)
                .GreaterThanOrEqualTo(1).WithMessage("There must be at least 1 bedroom.");
        }
    }
}
