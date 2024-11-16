using AutoMapper;
using BookingWebApi.Application.Apartament.DTOs;
using BookingWebApi.Application.Apartament.Interfaces;
using BookingWebApi.Application.Common.Decorators;
using BookingWebApi.Application.Common.Models;
using BookingWebApi.Application.Common.Response;
using BookingWebApi.Application.User.Interfaces;
using BookingWebApi.Domain.Constants;
using BookingWebApi.Domain.Entities;
using ApartamentEntity = BookingWebApi.Domain.Entities.Apartament;

namespace BookingWebApi.Application.Apartament.Services
{
    public interface IApartamentService
    {
        Task<Result<ApartamentPostDto>> CreateApartament(ApartamentPostDto apartamentDto, string UserId);
        Task<PageResultResponse<ApartamentGetDto>> GetApartaments(ApartamentFilter filter);
    }
    public class ApartamentService(
           IApartamentRepository _apartamentRepository,
           IAppUserRepository _appUserRepository,
           IMapper _mapper,
           IUserManagerDecorator<AppUser> _userManager
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

    }
}
