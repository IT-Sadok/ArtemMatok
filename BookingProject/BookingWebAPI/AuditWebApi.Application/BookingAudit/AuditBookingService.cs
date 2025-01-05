using AuditWebApi.Domain.Entities;
using AutoMapper;
using Contracts.DTOs.Audit;
using Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditWebApi.Application.BookingAudit
{
    public interface IAuditBookingService
    {
        Task<Result<bool>> CreateAuditBooking(AuditBookingCreateDto auditBookingDto);
        Task<Result<bool>> RemoveAuditBookingAsync(RemoveAuditBookingDto auditBookingDto);
    }

    public class AuditBookingService(
        IAuditBookingRepository _auditBookingRepository,
        IMapper _mapper
    ) : IAuditBookingService
    {
        public async Task<Result<bool>> CreateAuditBooking(AuditBookingCreateDto auditBookingDto)
        {
            var auditBooking = _mapper.Map<AuditBooking>(auditBookingDto);

            var result = await _auditBookingRepository.AddAuditBookingAsync(auditBooking);
            if (!result.IsSuccess) return Result<bool>.Failure(result.ErrorMessage);

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> RemoveAuditBookingAsync(RemoveAuditBookingDto auditBookingDto)
        {
            return await _auditBookingRepository.RemoveAuditBookingAsync(auditBookingDto.UserId, auditBookingDto.ApartamentId, auditBookingDto.StartDate);
        }
    }
}
