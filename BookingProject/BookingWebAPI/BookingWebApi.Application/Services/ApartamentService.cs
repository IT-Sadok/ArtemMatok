using AutoMapper;
using BookingWebApi.Application.Decorators;
using BookingWebApi.Application.DTOs.ApartamentDTOs;
using BookingWebApi.Application.Filters;
using BookingWebApi.Application.Interfaces;
using BookingWebApi.Application.Models;
using BookingWebApi.Application.Response;
using BookingWebApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Application.Services
{
    public class ApartamentService (
        IApartamentRepository _apartamentRepository,
        IAppUserRepository _appUserRepository,
        IMapper _mapper
    ) : IApartamentService
    {
        public async Task<Result<ApartamentPostDto>> CreateApartament(ApartamentPostDto apartamentDto, string userId)
        {
            if(String.IsNullOrWhiteSpace(userId))
            {
                return Result<ApartamentPostDto>.Failure("User id is required");
            }

            if(!await _appUserRepository.IsUserExist(userId))
            {
                return Result<ApartamentPostDto>.Failure("User wasn`t found");
            }

            var apartament = _mapper.Map<Apartament>(apartamentDto);
            apartament.HostId = userId;

            var result = await _apartamentRepository.CreateApartament(apartament);

            if(!result.IsSuccess)
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
