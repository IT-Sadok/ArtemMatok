using BookingWebApi.Application.ApartamentFeature.Interfaces;
using BookingWebApi.Application.ApartamentFeature.StatisticFeature.StatisticDTOs;
using BookingWebApi.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Application.ApartamentFeature.StatisticFeature
{
    public class ApartamentStatisticService(
        IApartamentRepository _apartamentRepository
    ) : IApartamentStatisticService
    {
        public async Task<Result<AreaQuantilesDto>> GetAreaQuantiles()
        {
            return await _apartamentRepository.GetAreaQuantiles();
        }

        public async Task<Result<List<BedroomStatisticsDto>>> GetAverageAreaByBedrooms()
        {
            return await _apartamentRepository.GetAverageAreaByBedrooms();
        }

        public async Task<Result<List<HostLargeApartmentDto>>> GetHostLargeAvarageApartament()
        {
            return await _apartamentRepository.GetHostLargeAvarageApartament();
        }

        public async Task<Result<decimal>> GetMedianArea()
        {
            var result = await _apartamentRepository.GetMedianArea();

            return Result<decimal>.Success(result.Value);
        }

        public async Task<Result<List<TotalAreaCountBySourceDto>>> GetTotalAreaCountBySourceCompany()
        {
            return await _apartamentRepository.GetTotalAreaCountBySourceCompany();
        }
    }
}
