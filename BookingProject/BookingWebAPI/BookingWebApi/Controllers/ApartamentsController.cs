using BookingWebApi.Application.Interfaces;
using BookingWebApi.Domain.Constants;
using BookingWebApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookingWebApi.Application.Common.Extensions;
using BookingWebApi.Application.Apartament.DTOs;
using BookingWebApi.Application.Apartament.Interfaces;
using BookingWebApi.Application.Apartament;
using BookingWebApi.Application.Apartament.Statistics;

namespace BookingWebApi.Controllers
{
    [Route("api/apartments")]
    [ApiController]
    public class ApartamentsController(
        IApartamentService _apartamentService,
        IApartamentStatisticService _apartamentStatisticService
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

        [HttpGet("median-area")]
        public async Task<IActionResult> GetMedianArea()
        {
            var result = await _apartamentStatisticService.GetMedianArea();

            return result.ToResponse();
        }

        [HttpGet("avarage-area-by-bedrooms")]
        public async Task<IActionResult> GetAvarangeAreaByBedrooms()
        {
            var result = await _apartamentStatisticService.GetAverageAreaByBedrooms();
            return result.ToResponse();
        }

        [HttpGet("host-large-avarage-apartaments")]
        public async Task<IActionResult> GetHostWithLargeAvarangeApartaments()
        {
            var result = await _apartamentStatisticService.GetHostLargeAvarageApartament();

            return result.ToResponse();
        }

        [HttpGet("total-area-count-by-source-company")]
        public async Task<IActionResult> GetTotalAreaCountBySourceCompany()
        {
            var result = await _apartamentStatisticService.GetTotalAreaCountBySourceCompany();
            return result.ToResponse(); 
        }

        [HttpGet("area-quantiels")]
        public async Task<IActionResult> GetAreaQuantiels()
        {
            var result = await _apartamentStatisticService.GetAreaQuantiles();
            return result.ToResponse();
        }
    }
}
