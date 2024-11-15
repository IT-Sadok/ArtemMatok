using BookingWebApi.Application.ApartamentFeature.DTOs;
using BookingWebApi.Application.Common.Models;
using BookingWebApi.Application.Common.Response;


namespace BookingWebApi.Application.ApartamentFeature.Interfaces
{
    public interface IApartamentService
    {
        Task<Result<ApartamentPostDto>> CreateApartament(ApartamentPostDto apartamentDto, string UserId);
        Task<PageResultResponse<ApartamentGetDto>> GetApartaments(ApartamentFilter filter);

    }
}
