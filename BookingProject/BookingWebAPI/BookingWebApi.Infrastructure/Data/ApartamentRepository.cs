using BookingWebApi.Application.Apartament.Interfaces;
using BookingWebApi.Application.ApartamentFeature;
using BookingWebApi.Application.ApartamentFeature.StatisticFeature.StatisticDTOs;
using BookingWebApi.Application.Common.Models;
using BookingWebApi.Application.Common.Response;
using BookingWebApi.Application.Interfaces;
using BookingWebApi.Domain.Entities;
using BookingWebApi.Infrastructure.Configuration;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Collections.Generic;
using System.Resources;




namespace BookingWebApi.Infrastructure.Data
{
    public class ApartamentRepository : IApartamentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly string _connectionString;
        private const string _sqlScriptsPath = "BookingWebApi.Infrastructure.SqlScripts.ApartamentSql.ApartmentSqlResources";

        public ApartamentRepository(ApplicationDbContext context, string connectionString, IOptions<SqlSettings> options)
        {
            _context = context;
            _connectionString = connectionString;
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

            var resourceManager = new ResourceManager(_sqlScriptsPath, typeof(ApartamentRepository).Assembly);

            var sql = resourceManager.GetString("AreaQuantiles");
            if (string.IsNullOrEmpty(sql))
            {
                return Result<AreaQuantilesDto>.Failure("Failure loaded sql file");
            }

            try
            {
                var result = await connection.QuerySingleAsync<AreaQuantilesDto>(sql);
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

            var resourceManager = new ResourceManager(_sqlScriptsPath, typeof(ApartamentRepository).Assembly);

            var sql = resourceManager.GetString("AverageAreaByBedrooms");
            if (string.IsNullOrEmpty(sql))
            {
                return Result<List<BedroomStatisticsDto>>.Failure("Failure loaded sql file");
            }

            try
            {
                var result = await connection.QueryAsync<BedroomStatisticsDto>(sql);
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

            var resourceManager = new ResourceManager(_sqlScriptsPath, typeof(ApartamentRepository).Assembly);
            var sql = resourceManager.GetString("HostsWithLargeAverageApartments");
            if (string.IsNullOrEmpty(sql))
            {
                return Result<List<HostLargeApartmentDto>>.Failure("Failure loaded sql file");
            }

            try
            {
                var result = await connection.QueryAsync<HostLargeApartmentDto>(sql);
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

            var resourceManager = new ResourceManager(_sqlScriptsPath, typeof(ApartamentRepository).Assembly);

            var sql = resourceManager.GetString("MedianArea");
            if (string.IsNullOrEmpty(sql))
            {
                return Result<decimal>.Failure("Failure loaded sql file");
            }

            try
            {
                var result = await connection.QuerySingleAsync<decimal>(sql);
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

            var resourceManager = new ResourceManager(_sqlScriptsPath, typeof(ApartamentRepository).Assembly);
            var sql = resourceManager.GetString("TotalAreaAndCountBySourceCompany");
            if (string.IsNullOrEmpty(sql))
            {
                return Result<List<TotalAreaCountBySourceDto>>.Failure("Failure loaded sql file");
            }

            try
            {
                var result = await connection.QueryAsync<TotalAreaCountBySourceDto>(sql);
                return Result<List<TotalAreaCountBySourceDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<TotalAreaCountBySourceDto>>.Failure(ex.Message);
            }
        }
    }
}


