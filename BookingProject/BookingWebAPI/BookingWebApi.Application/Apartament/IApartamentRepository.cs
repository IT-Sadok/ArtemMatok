using BookingWebApi.Application.Apartament.Statistics.StatisticDTOs;
using BookingWebApi.Application.Common.Models;
using BookingWebApi.Application.Common.Response;
using BookingWebApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApartamentEntity = BookingWebApi.Domain.Entities.Apartament;


namespace BookingWebApi.Application.Apartament
{
    public interface IApartamentRepository
    {
        Task<bool> ApartamentExist(int apartamentId);
        Task<Result<ApartamentEntity>> CreateApartament(ApartamentEntity apartament);
        Task<PageResultResponse<ApartamentEntity>> GetApartamets(ApartamentFilter filter);
        Task<Result<decimal>> GetMedianArea();
        Task<Result<List<BedroomStatisticsDto>>> GetAverageAreaByBedrooms();
        Task<Result<List<HostLargeApartmentDto>>> GetHostLargeAvarageApartament();
        Task<Result<List<TotalAreaCountBySourceDto>>> GetTotalAreaCountBySourceCompany();
        Task<Result<AreaQuantilesDto>> GetAreaQuantiles();
        Task<Result<bool>> UpsertCustomData(int apartamentId, string customData);
    }
}
