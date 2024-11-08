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
    public interface IApartamentService
    {
        Task<Result<ApartamentPostDto>> CreateApartament(ApartamentPostDto apartamentDto, string UserId);
        Task<PageResultResponse<ApartamentGetDto>> GetApartaments(ApartamentFilter filter);
    }
}
