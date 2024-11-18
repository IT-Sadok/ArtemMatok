using BookingWebApi.Application.Apartament;
using BookingWebApi.Application.Apartament.Statistics;
using BookingWebApi.Application.Apartament.Statistics.StatisticDTOs;
using BookingWebApi.Application.Common.Models;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Tests.Services
{
    public class ApartamentStatisticServiceTests
    {
        public readonly Mock<IApartamentRepository> _apartamentRepository;
        private readonly ApartamentStatisticService _apartamentStatisticsService;
        public ApartamentStatisticServiceTests()
        {
            _apartamentRepository = new Mock<IApartamentRepository>();
            _apartamentStatisticsService = new ApartamentStatisticService(
                _apartamentRepository.Object
            );
        }

        [Fact]
        public async Task ApartamentService_GetAreaQuantiles_Success()
        {

            var areaQuantilies = new AreaQuantilesDto(12, 13, 15);

            _apartamentRepository.Setup(x => x.GetAreaQuantiles())
                .ReturnsAsync(Result<AreaQuantilesDto>.Success(areaQuantilies));

            var res = await _apartamentStatisticsService.GetAreaQuantiles();

            res.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task ApartamentService_GetAverageAreaByBedrooms_Success()
        {
            var bedroomStatisticsList = new List<BedroomStatisticsDto>()
            {
                new BedroomStatisticsDto(1,50),
                new BedroomStatisticsDto(2,80),
                new BedroomStatisticsDto(3,120),
            };

            _apartamentRepository.Setup(x => x.GetAverageAreaByBedrooms())
                .ReturnsAsync(Result<List<BedroomStatisticsDto>>.Success(bedroomStatisticsList));

            var result = await _apartamentStatisticsService.GetAverageAreaByBedrooms();

            result.IsSuccess.Should().Be(true);
            result.Value.Should().BeOfType<List<BedroomStatisticsDto>>();
        }

        [Fact]
        public async Task ApartamentService_GetHostLargeAvarageApartament_Success()
        {
            var hostLargeApartments = new List<HostLargeApartmentDto>
            {
                new HostLargeApartmentDto("id1",2,25),
                new HostLargeApartmentDto("id2",3,24)
            };

            _apartamentRepository.Setup(x => x.GetHostLargeAvarageApartament())
                .ReturnsAsync(Result<List<HostLargeApartmentDto>>.Success(hostLargeApartments));

            var result = await _apartamentStatisticsService.GetHostLargeAvarageApartament();

            result.IsSuccess.Should().BeTrue();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task ApartamentService_GetMedianArea_Success()
        {
            decimal median = 45;

            _apartamentRepository.Setup(x => x.GetMedianArea())
                .ReturnsAsync(Result<decimal>.Success(median));

            var result = await _apartamentStatisticsService.GetMedianArea();

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(45);
        }


        [Fact]
        public async Task ApartamentService_GetTotalAreaCountBySourceCompany_Success()
        {
            var list = new List<TotalAreaCountBySourceDto>()
            {
                new TotalAreaCountBySourceDto("company1",123,5),
                new TotalAreaCountBySourceDto("company2",100,2),
            };

            _apartamentRepository.Setup(x => x.GetTotalAreaCountBySourceCompany())
                .ReturnsAsync(Result<List<TotalAreaCountBySourceDto>>.Success(list));

            var result = await _apartamentStatisticsService.GetTotalAreaCountBySourceCompany();

            result.IsSuccess.Should().BeTrue();

        }
    }
}
