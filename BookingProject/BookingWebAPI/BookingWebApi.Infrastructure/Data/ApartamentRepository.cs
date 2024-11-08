using BookingWebApi.Application.Filters;
using BookingWebApi.Application.Interfaces;
using BookingWebApi.Application.Models;
using BookingWebApi.Application.Response;
using BookingWebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Infrastructure.Data
{
    public class ApartamentRepository(
        ApplicationDbContext _context
    ) : IApartamentRepository
    {
        public async Task<Result<Apartament>> CreateApartament(Apartament apartament)
        {
            try
            {
                await _context.Apartaments.AddAsync(apartament);    
                await _context.SaveChangesAsync();
                return Result<Apartament>.Success(apartament);
            }
            catch (Exception ex)
            {
                return Result<Apartament>.Failure(ex.Message);
            } 
        }

        public async Task<PageResultResponse<Apartament>> GetApartamets(ApartamentFilter filter)
        {
            var apartaments =  _context.Apartaments.AsQueryable();

            int totalCount = await apartaments.CountAsync() ;

            if (filter.MinBedrooms.HasValue)
            {
                apartaments = apartaments.Where(x => x.Bedrooms >= filter.MinBedrooms.Value);
            }

            if(filter.MaxArea.HasValue)
            {
                apartaments = apartaments.Where(x => x.Area <= filter.MaxArea.Value);
            }

            apartaments = apartaments
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize);

            var apartamentsList = await apartaments
                .Include(x=>x.Host)
                .ToListAsync();

           

            return new PageResultResponse<Apartament>(apartamentsList, totalCount, filter.PageNumber, filter.PageSize);
        }
    }
}
