using AutoMapper;
using BookingWebApi.Application.Apartament;
using BookingWebApi.Application.Apartament.DTOs;

using BookingWebApi.Application.Common.Decorators;
using BookingWebApi.Application.User.Interfaces;
using BookingWebApi.Domain.Constants;
using BookingWebApi.Domain.Entities;
using FluentAssertions;
using Moq;
using Response;

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
        public async Task ApartamentService_UpsertCustomData_Success()
        {
            int apartamentId = 1;
            string userId = "testId";
            List<ApartamentCustomDataDto> newCustomData = new List<ApartamentCustomDataDto>()
            {
                new ApartamentCustomDataDto("TestKey","TestValue")
            };


            _appUserRepository.Setup(x => x.UserExists(userId))
                .ReturnsAsync(true);
            _userManager.Setup(x => x.IsInRoleAsync(userId, UserRoles.Host))
                .ReturnsAsync(true);
            _apartamentRepository.Setup(x => x.ApartamentExist(apartamentId))
                .ReturnsAsync(true);
            _apartamentRepository.Setup(x => x.UpsertCustomData(apartamentId, It.IsAny<string>()))
                .ReturnsAsync(Result<bool>.Success(true));

            var result = await _apartamentService.UpsertCustomData(apartamentId, newCustomData, userId);

            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task ApartamentService_UpsertCustomData_UserIdNull_Failure()
        {
            int apartamentId = 1;
            List<ApartamentCustomDataDto> newCustomData = new List<ApartamentCustomDataDto>()
            {
                new ApartamentCustomDataDto("TestKey","TestValue")
            };

            var result = await _apartamentService.UpsertCustomData(apartamentId, newCustomData, null);

            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse(); 
        }

        [Fact]
        public async Task ApartamentService_UpsertCustomData_RoleIncorrect_Failure()
        {
            int apartamentId = 1;
            string userId = "testId";
            List<ApartamentCustomDataDto> newCustomData = new List<ApartamentCustomDataDto>()
            {
                new ApartamentCustomDataDto("TestKey","TestValue")
            };

            _appUserRepository.Setup(x => x.UserExists(userId))
                .ReturnsAsync(true);
            _userManager.Setup(x => x.IsInRoleAsync(userId, UserRoles.Host))
                .ReturnsAsync(false);
            var result = await _apartamentService.UpsertCustomData(apartamentId, newCustomData, null);

            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public async Task ApartamentService_UpsertCustomData_ApartamentIsNotExist_Failure()
        {
            int apartamentId = 1;
            string userId = "testId";
            List<ApartamentCustomDataDto> newCustomData = new List<ApartamentCustomDataDto>()
            {
                new ApartamentCustomDataDto("TestKey","TestValue")
            };

            _appUserRepository.Setup(x => x.UserExists(userId))
                .ReturnsAsync(true);
            _userManager.Setup(x => x.IsInRoleAsync(userId, UserRoles.Host))
                .ReturnsAsync(true);
            _apartamentRepository.Setup(x => x.UpsertCustomData(apartamentId, It.IsAny<string>()))
                .ReturnsAsync(Result<bool>.Success(true));

            var result = await _apartamentService.UpsertCustomData(apartamentId, newCustomData, null);

            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public async Task ApartamentService_UspertCustomData_UpsertWithFailure_Failure()
        {
            int apartamentId = 1;
            string userId = "testId";
            List<ApartamentCustomDataDto> newCustomData = new List<ApartamentCustomDataDto>()
            {
                new ApartamentCustomDataDto("TestKey","TestValue")
            };


            _appUserRepository.Setup(x => x.UserExists(userId))
                .ReturnsAsync(true);
            _userManager.Setup(x => x.IsInRoleAsync(userId, UserRoles.Host))
                .ReturnsAsync(true);
            _apartamentRepository.Setup(x => x.ApartamentExist(apartamentId))
                .ReturnsAsync(true);
            _apartamentRepository.Setup(x => x.UpsertCustomData(apartamentId, It.IsAny<string>()))
                .ReturnsAsync(Result<bool>.Failure("Problems with sql code"));

            var result = await _apartamentService.UpsertCustomData(apartamentId, newCustomData, userId);

            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Problems with sql code");
        }
    }
}
