using BookingWebApi.Application.DTOs.ApartamentDTOs;
using BookingWebApi.Application.Extensions;
using BookingWebApi.Application.Filters;
using BookingWebApi.Application.Interfaces;
using BookingWebApi.Domain.Constants;
using BookingWebApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace BookingWebApi.Controllers
{
    [Route("api/apartments")]
    [ApiController]
    public class ApartamentsController(
        IApartamentService _apartamentService    
    ) : ControllerBase
    {
        [Authorize(Roles = UserRoles.Host)]
        [HttpPost]
        public async Task<IActionResult> CreateApartament(ApartamentPostDto apartamentDto)
        {
            var result = await _apartamentService.CreateApartament(apartamentDto, UserHelpers.GetUserId(HttpContext));

            return result.ToResponse();
        }

        
        [HttpGet]
        public async Task<IActionResult> GetApartaments([FromQuery] ApartamentFilter filter)
        {
            var result = await _apartamentService.GetApartaments(filter);

            return Ok(result);
        }
    }
}
