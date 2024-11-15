using AutoMapper;
using BookingWebApi.Application.ApartamentFeature.DTOs;
using BookingWebApi.Application.ApartamentFeature.StatisticFeature.StatisticDTOs;
using BookingWebApi.Application.Common.Decorators;
using BookingWebApi.Application.Common.Models;
using BookingWebApi.Application.Filters;
using BookingWebApi.Application.Interfaces;
using BookingWebApi.Application.Response;
using BookingWebApi.Application.Services;
using BookingWebApi.Domain.Constants;
using BookingWebApi.Domain.Entities;
using FluentAssertions;
using Moq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Tests.Services
{
    public class ApartamentServiceTests
    {
        private readonly Mock<IApartamentRepository> _apartamentRepository;
        private readonly Mock<IAppUserRepository> _appUserRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IUserManagerDecorator<AppUser>> _userManager;
        private readonly ApartamentService _apartamentService;

        public ApartamentServiceTests()
        {
            _apartamentRepository = new Mock<IApartamentRepository>();  
            _appUserRepository = new Mock<IAppUserRepository>();
            _userManager = new Mock<IUserManagerDecorator<AppUser>>();

            _mapper = new Mock<IMapper>();
            _apartamentService = new ApartamentService(
                _apartamentRepository.Object,
                _appUserRepository.Object,
                _mapper.Object,
                _userManager.Object
            );
        }

        [Fact]
        public async Task ApartamentService_CreateApartament_Success()
        {
            var apartamentDto = new ApartamentPostDto(
                "TestAddress",
                10,
                12,
                12,
                3
            );
            string userId = "testId";
            var apartament = new Apartament()
            {
                Address = apartamentDto.Address,
                Area = apartamentDto.Area,
                Longitude = apartamentDto.Longtitude,
                Latitude = apartamentDto.Latitude,
                Bedrooms = apartamentDto.Bedrooms,
                HostId = userId
            };

            var resultApartament = Result<Apartament>.Success(new Apartament { ApartamentId = 1, HostId = userId });

            _appUserRepository.Setup(x => x.UserExists(userId))
                .ReturnsAsync(true);
            _userManager.Setup(x => x.IsInRoleAsync(userId, UserRoles.Host))
                .ReturnsAsync(true);
            _mapper.Setup(x => x.Map<Apartament>(It.IsAny<ApartamentPostDto>()))
                .Returns(apartament);
            _apartamentRepository.Setup(x => x.CreateApartament(It.IsAny<Apartament>()))
                .ReturnsAsync(resultApartament);


            var result = await _apartamentService.CreateApartament(apartamentDto, userId);


            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();  
        }

        [Fact]
        public async Task ApartamentService_CreateApartament_WhenUserIdIsEmpty_ReturnFailure()
        {
            var apartamentDto = new ApartamentPostDto(
                "TestAddress",
                10,
                24,
                12,
                3
            );
            var userId = "";

            var result = await _apartamentService.CreateApartament(apartamentDto, userId);

            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("User id is required");
            
        }

        [Fact]
        public async Task ApartamentService_CreateApartament_WhenUserNotExist_ReturnFailure()
        {
            var apartamentDto = new ApartamentPostDto(
                "TestAddress",
                10,
                23,
                12,
                3
            );
            string userId = "testId";
            var apartament = new Apartament()
            {
                Address = apartamentDto.Address,
                Area = apartamentDto.Area,
                Longitude = apartamentDto.Longtitude,
                Latitude = apartamentDto.Latitude,
                Bedrooms = apartamentDto.Bedrooms,
                HostId = userId
            };

            _appUserRepository.Setup(x => x.UserExists(userId))
                .ReturnsAsync(false);

            var result = await _apartamentService.CreateApartament(apartamentDto,userId);

            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("User wasn`t found");
        }

        [Fact]
        public async Task ApartamentService_GetApartaments_ReturnSuccess()
        {
            var filter = new ApartamentFilter()
            {
                MaxArea = 150,
                MinBedrooms = 1,
                PageNumber = 1,
            };
            var apartamentList = new List<Apartament>
            {
                new Apartament { ApartamentId = 1, Address = "Test Address 1", Area=100, Bedrooms=2},
                new Apartament { ApartamentId = 2, Address = "Test Address 2", Area=140, Bedrooms=3 }
            };
            var pageResult = new PageResultResponse<Apartament>(apartamentList, 2, 1, 10);
            var apartamentGetDtoList = new List<ApartamentGetDto>
            {
                new ApartamentGetDto { ApartamentId = 1, Area=100, Bedrooms=2 },
                new ApartamentGetDto { ApartamentId = 2, Bedrooms=3 }
            };

            _apartamentRepository.Setup(x => x.GetApartamets(filter))
                .ReturnsAsync(pageResult);

            _mapper.Setup(x => x.Map<List<ApartamentGetDto>>(apartamentList))
                .Returns(apartamentGetDtoList);

            var result = await _apartamentService.GetApartaments(filter);

            result.Should().NotBeNull();
            result.Items.Should().HaveCount(2);
        }

        [Fact]
        public async Task ApartamentService_GetAreaQuantiles_Success()
        {

            var areaQuantilies = new AreaQuantilesDto(12, 13, 15);
            
            _apartamentRepository.Setup(x => x.GetAreaQuantiles())
                .ReturnsAsync(Result<AreaQuantilesDto>.Success(areaQuantilies));

            var res = await _apartamentService.GetAreaQuantiles();

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

            var result = await _apartamentService.GetAverageAreaByBedrooms();

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

            var result = await _apartamentService.GetHostLargeAvarageApartament();

            result.IsSuccess.Should().BeTrue() ;
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task ApartamentService_GetMedianArea_Success()
        {
            decimal median = 45;

            _apartamentRepository.Setup(x => x.GetMedianArea())
                .ReturnsAsync(Result<decimal>.Success(median));

            var result = await _apartamentService.GetMedianArea();

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

            var result = await _apartamentService.GetTotalAreaCountBySourceCompany();

            result.IsSuccess.Should().BeTrue();

        }
    }
}
