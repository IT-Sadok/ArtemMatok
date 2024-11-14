using BookingWebApi.Application.DTOs.ApartamentDTOs;
using BookingWebApi.Application.Filters;
using BookingWebApi.Application.Models;
using BookingWebApi.Application.Response;
using BookingWebApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Application.Interfaces
{
    public interface IApartamentRepository
    {
        Task<Result<Apartament>> CreateApartament(Apartament apartament);
        Task<PageResultResponse<Apartament>> GetApartamets(ApartamentFilter filter);
        Task<Result<decimal>> GetMedianArea();
        Task<Result<List<BedroomStatisticsDto>>> GetAverageAreaByBedrooms();
        Task<Result<List<HostLargeApartmentDto>>> GetHostLargeAvarageApartament();
        Task<Result<List<TotalAreaCountBySourceDto>>> GetTotalAreaCountBySourceCompany();
        Task<Result<AreaQuantilesDto>> GetAreaQuantiles();
    }
}
