using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Application.Booking
{
    public class BookingDtoValidator : AbstractValidator<BookingDto>
    {
        public BookingDtoValidator()
        {
            RuleFor(booking => booking.ApartamentId)
                .GreaterThan(0)
                .WithMessage("ApartamentId must be greater than 0.");

            RuleFor(booking => booking.StartDate)
                .LessThan(booking => booking.EndDate)
                .WithMessage("StartDate must be earlier than EndDate.");

            RuleFor(booking => booking.EndDate)
                .GreaterThan(DateTime.Now)
                .WithMessage("EndDate must be in the future.");
        }
    }
}
