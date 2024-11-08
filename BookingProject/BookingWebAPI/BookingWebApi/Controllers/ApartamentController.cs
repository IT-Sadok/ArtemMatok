using BookingWebApi.Application.DTOs.ApartamentDTOs;
using BookingWebApi.Application.Extensions;
using BookingWebApi.Application.Filters;
using BookingWebApi.Application.Interfaces;
using BookingWebApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace BookingWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApartamentController(
        IApartamentService _apartamentService    
    ) : ControllerBase
    {
        [Authorize(Roles ="Host")]
        [HttpPost("CreateApartament")]
        public async Task<IActionResult> CreateApartament(ApartamentPostDto apartamentDto)
        {
            var result = await _apartamentService.CreateApartament(apartamentDto, UserHelpers.GetUserId(HttpContext));

            return result.ToResponse();
        }

        
        [HttpGet("GetApartaments")]
        public async Task<IActionResult> GetApartaments([FromQuery] ApartamentFilter filter)
        {
            var result = await _apartamentService.GetApartaments(filter);

            return Ok(result);
        }
    }
}
