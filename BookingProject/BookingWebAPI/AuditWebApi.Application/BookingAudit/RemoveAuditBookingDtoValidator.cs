using Contracts.DTOs.Audit;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditWebApi.Application.BookingAudit
{
    public class RemoveAuditBookingDtoValidator : AbstractValidator<RemoveAuditBookingDto>
    {
        public RemoveAuditBookingDtoValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId us required");

            RuleFor(x => x.ApartamentId)
                .NotEmpty().WithMessage("ApartamentId is required");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("StartDate is required");
        }
    }
}
