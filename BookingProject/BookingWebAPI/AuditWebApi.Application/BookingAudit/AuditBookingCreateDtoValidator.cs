using Contracts.DTOs.Audit;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditWebApi.Application.BookingAudit
{
    public class AuditBookingCreateDtoValidator : AbstractValidator<AuditBookingCreateDto>
    {
        public AuditBookingCreateDtoValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required");

            RuleFor(x => x.ApartamentId)
                .NotEmpty().WithMessage("ApartamentId is required");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("StartDate is required");

            RuleFor(x => x.TotalAmount)
                .GreaterThan(1).WithMessage("TotalAmount greater than 1");

            RuleFor(x => x.CurrencyName)
                .NotEmpty().WithMessage("CurrenctName is required")
                .MinimumLength(2).WithMessage("Minimum length is 2");
        }
    }
}
