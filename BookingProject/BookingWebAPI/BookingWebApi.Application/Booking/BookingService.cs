using AutoMapper;
using BookingWebApi.Application.Apartament;
using Contracts.Clients;
using Contracts.DTOs.Audit;
using Contracts.DTOs.Payment;
using Microsoft.Extensions.Logging;
using Response;
using Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingEntity = BookingWebApi.Domain.Entities.Booking;

namespace BookingWebApi.Application.Booking
{
    public interface IBookingService
    {
        Task<Result<BookingDto>> CreateBooking(string userId, BookingDto bookingDto);
    }

    public class BookingService(
        IBookingRepository _bookingRepository,
        IApartamentRepository _apartamentRepository,
        IPaymentClient _paymentClient,
        IAuditClient _auditClient,
        IMapper _mapper,
        ILogger<BookingService> _logger
    ): IBookingService
    {
        public async Task<Result<BookingDto>> CreateBooking(string userId, BookingDto bookingDto)
        {
            var booking = _mapper.Map<BookingEntity>(bookingDto);
            booking.UserId = userId;

            var res = (await _apartamentRepository.CalculateTotalPriceWithCurrency(
                    bookingDto.ApartamentId, bookingDto.StartDate, bookingDto.EndDate));

            if (!res.IsSuccess) return Result<BookingDto>.Failure(res.ErrorMessage);

            booking.TotalAmount = res.Value.TotalPrice;
            booking.CurrencyName = res.Value.CurrencyName;
            booking.Status = "Creating";

            var balanceRequestDto = new BalanceRequestDto(userId, res.Value.TotalPrice, res.Value.CurrencyName);

            var saga = new SagaManager()
                .AddStep(
                    async () =>
                    {
                        var reserveBalance = await _paymentClient.ReserveBalance(balanceRequestDto);
                        return reserveBalance.IsSuccess
                            ? Result<object>.Success(null)
                            : Result<object>.Failure(reserveBalance.ErrorMessage);
                    },
                    compensations:[]
                )
                .AddStep(
                    async () =>
                    {
                        var auditBookingDto = new AuditBookingCreateDto(userId, bookingDto.ApartamentId, bookingDto.StartDate, res.Value.TotalPrice, res.Value.CurrencyName);

                        var auditBooking = await _auditClient.CreateAuditBooking(auditBookingDto);
                        return auditBooking.IsSuccess
                            ? Result<object>.Success(null)
                            : Result<object>.Failure(auditBooking.ErrorMessage);
                    },
                    compensations: new List<Func<Task>>
                    {
                        async () => await _paymentClient.CompensateBalance(balanceRequestDto)
                    }
                )
                .AddStep(
                    async () =>
                    {
                        booking.Status = "Successful";
                        var createBooking = await _bookingRepository.CreateBookingAsync(booking);
                        return createBooking.IsSuccess
                            ? Result<object>.Success(null)
                            : Result<object>.Failure(createBooking.ErrorMessage);
                    },
                    compensations:new List<Func<Task>>
                    {
                        async () => await _auditClient.RemoveAuditBooking(new RemoveAuditBookingDto(userId, bookingDto.ApartamentId, bookingDto.StartDate))
                    }
                );

            var result = await saga.ExecuteAsync();
            if (!result.IsSuccess)
            {
                return Result<BookingDto>.Failure("Booking failed due to one or more errors: " + result.ErrorMessage);
            }

            return Result<BookingDto>.Success(bookingDto);
        }
    }
}
