using BookingWebApi.Application.Apartament;
using BookingWebApi.Application.Apartament.DTOs;
using BookingWebApi.Application.Apartament.Statistics.StatisticDTOs;
using BookingWebApi.Domain.Entities;
using BookingWebApi.Infrastructure.Configuration;
using BookingWebApi.Infrastructure.SqlScripts;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Response;
using System.Resources;
using System.Text.Json;




namespace BookingWebApi.Infrastructure.Data
{
    public class ApartamentRepository : IApartamentRepository
    {
        private readonly ApplicationDbContext _context;

        public ApartamentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

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
                .ToListAsync();

            return new PageResultResponse<Apartament>(apartamentsList, totalCount, filter.PageNumber, filter.PageSize);
        }

        public async Task<Result<AreaQuantilesDto>> GetAreaQuantiles()
        {
            await using var connection = _context.Database.GetDbConnection();

            try
            {
                var result = await connection.QuerySingleAsync<AreaQuantilesDto>(Resources.AreaQuantiles);
                return Result<AreaQuantilesDto>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<AreaQuantilesDto>.Failure(ex.Message);
            }
        }

        public async Task<Result<List<BedroomStatisticsDto>>> GetAverageAreaByBedrooms()
        {
            await using var connection = _context.Database.GetDbConnection();

            try
            {
                var result = await connection.QueryAsync<BedroomStatisticsDto>(Resources.AverageAreaByBedrooms);
                return Result<List<BedroomStatisticsDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<BedroomStatisticsDto>>.Failure(ex.Message);
            }
        }

        public async Task<Result<List<HostLargeApartmentDto>>> GetHostLargeAvarageApartament()
        {
            await using var connection = _context.Database.GetDbConnection();

            try
            {
                var result = await connection.QueryAsync<HostLargeApartmentDto>(Resources.HostsWithLargeAverageApartments);
                return Result<List<HostLargeApartmentDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<HostLargeApartmentDto>>.Failure(ex.Message);
            }
        }

        public async Task<Result<decimal>> GetMedianArea()
        {
            await using var connection = _context.Database.GetDbConnection();

            try
            {
                var result = await connection.QuerySingleAsync<decimal>(Resources.MedianArea);
                return Result<decimal>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<decimal>.Failure(ex.Message);
            } 
        }

        public async Task<Result<List<TotalAreaCountBySourceDto>>> GetTotalAreaCountBySourceCompany()
        {
            await using var connection = _context.Database.GetDbConnection();

            try
            {
                var result = await connection.QueryAsync<TotalAreaCountBySourceDto>(Resources.TotalAreaAndCountBySourceCompany);
                return Result<List<TotalAreaCountBySourceDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<TotalAreaCountBySourceDto>>.Failure(ex.Message);
            }
        }

        public async Task<Result<bool>> UpsertCustomData<T>(int apartamentId, T customData)
        {
            await using var connection = _context.Database.GetDbConnection();

            try
            {
                var result = await connection.ExecuteAsync(Resources.Upsert,new {ApartamentId = apartamentId, CustomData = customData});
                if(result == 0)
                {
                    return Result<bool>.Failure("Problems with sql code");
                }
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message);
            }
        }

        public async Task<bool> ApartamentExist(int apartamentId)
        {
            var apartament = await _context.Apartaments.FindAsync(apartamentId);

            if (apartament == null) return false;
            return true;
        }

        public async Task<Result<TotalPriceWithCurrencyDto>> CalculateTotalPriceWithCurrency(int apartamentId, DateTime startDate, DateTime endDate)
        {
            var apartament = await _context.Apartaments.FindAsync(apartamentId);

            if(apartament is null)
            {
                return Result<TotalPriceWithCurrencyDto>.Failure("Apartament wasn`t found");
            }

            var totalDays = (endDate - startDate).Days;

            var totalPrice = totalDays * apartament.PricePerDay;

            return Result<TotalPriceWithCurrencyDto>.Success(new TotalPriceWithCurrencyDto(totalPrice, apartament.CurrencyName));
        }
    }
}


