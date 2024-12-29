using AutoMapper;
using BookingWebApi.Application.Apartament.DTOs;
using BookingWebApi.Application.Common.Decorators;
using BookingWebApi.Application.User.Interfaces;
using BookingWebApi.Domain.Constants;
using BookingWebApi.Domain.Entities;
using Microsoft.Extensions.Logging;
using Response;
using System.Text.Json;
using ApartamentEntity = BookingWebApi.Domain.Entities.Apartament;

namespace BookingWebApi.Application.Apartament
{
    public interface IApartamentService
    {
        Task<Result<ApartamentPostDto>> CreateApartament(ApartamentPostDto apartamentDto, string UserId);
        Task<PageResultResponse<ApartamentGetDto>> GetApartaments(ApartamentFilter filter);
        Task<Result<string>> UpsertCustomData(int apartamentId, List<ApartamentCustomDataDto> newCustomData, string userId);
    }

    public class ApartamentService(
           IApartamentRepository _apartamentRepository,
           IAppUserRepository _appUserRepository,
           IMapper _mapper,
           IUserManagerDecorator<AppUser> _userManager,
           ILogger<ApartamentService> _logger
       ) : IApartamentService
    {
        public async Task<Result<ApartamentPostDto>> CreateApartament(ApartamentPostDto apartamentDto, string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Result<ApartamentPostDto>.Failure("User id is required");
            }

            if (!await _appUserRepository.UserExists(userId))
            {
                return Result<ApartamentPostDto>.Failure("User wasn`t found");
            }

            if (!await _userManager.IsInRoleAsync(userId, UserRoles.Host))
            {
                return Result<ApartamentPostDto>.Failure("User does not have permission to create an apartment");
            }

            var apartament = _mapper.Map<ApartamentEntity>(apartamentDto);
            apartament.HostId = userId;

            var result = await _apartamentRepository.CreateApartament(apartament);

            if (!result.IsSuccess)
            {
                return Result<ApartamentPostDto>.Failure(result.ErrorMessage);
            }

            ApartamentCustomMetrics.SuccessfulApartamentCreations.Inc();
            _logger.LogInformation("Incrementing successful_apartament_creations_total");

            return Result<ApartamentPostDto>.Success(apartamentDto);
        }

        public async Task<PageResultResponse<ApartamentGetDto>> GetApartaments(ApartamentFilter filter)
        {
            var result = await _apartamentRepository.GetApartamets(filter);

            var apartamentsDto = _mapper.Map<List<ApartamentGetDto>>(result.Items);

            return new PageResultResponse<ApartamentGetDto>(
               apartamentsDto,
               result.TotalCount,
               result.CurrentPage,
               result.PageSize
            );
        }

        public async Task<Result<string>> UpsertCustomData(int apartamentId, List<ApartamentCustomDataDto> newCustomData, string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Result<string>.Failure("User id is required");
            }

            if (!await _appUserRepository.UserExists(userId))
            {
                return Result<string>.Failure("User wasn`t found");
            }

            if (!await _userManager.IsInRoleAsync(userId, UserRoles.Host))
            {
                return Result<string>.Failure("User does not have permission to create an apartment");
            }

            if (!await _apartamentRepository.ApartamentExist(apartamentId))
            {
                return Result<string>.Failure("Apartament wasn`t found");
            }

            var dictionary = newCustomData.ToDictionary(cd => cd.Key, cd => cd.Value);

            var customData = JsonSerializer.Serialize(dictionary);

            if(string.IsNullOrWhiteSpace(customData))
            {
                return Result<string>.Failure("Problems with serialization");
            }

            var result = await _apartamentRepository.UpsertCustomData(apartamentId, customData);

            if (!result.IsSuccess)
            {
                return Result<string>.Failure(result.ErrorMessage);
            }
            return Result<string>.Success("Custom data added/updated");
        }
    }
}
