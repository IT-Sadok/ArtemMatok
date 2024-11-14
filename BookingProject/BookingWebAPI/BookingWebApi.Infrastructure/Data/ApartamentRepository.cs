using BookingWebApi.Application.DTOs.ApartamentDTOs;
using BookingWebApi.Application.Filters;
using BookingWebApi.Application.Interfaces;
using BookingWebApi.Application.Models;
using BookingWebApi.Application.Response;
using BookingWebApi.Domain.Entities;
using BookingWebApi.Infrastructure.Configuration;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Npgsql;




namespace BookingWebApi.Infrastructure.Data
{
    public class ApartamentRepository : IApartamentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly string _connectionString;
        private readonly string _sqlScriptsPath;

        public ApartamentRepository(ApplicationDbContext context, string connectionString, IOptions<SqlSettings> options)
        {
            _context = context;
            _connectionString = connectionString;
            _sqlScriptsPath = Path.Combine(
                Directory.GetParent(Directory.GetCurrentDirectory()).FullName,
                options.Value.SqlScriptsPath);
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
            await using var connection = new NpgsqlConnection(_connectionString);

            var sql = await LoadSql("AreaQuantiles.sql");
            if (!sql.IsSuccess)
            {
                return Result<AreaQuantilesDto>.Failure(sql.ErrorMessage);
            }

            try
            {
                var result = await connection.QuerySingleAsync<AreaQuantilesDto>(sql.Value);
                return Result<AreaQuantilesDto>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<AreaQuantilesDto>.Failure(ex.Message);
            }
        }

        public async Task<Result<List<BedroomStatisticsDto>>> GetAverageAreaByBedrooms()
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            var sql = await LoadSql("AverageAreaByBedrooms.sql");
            if(!sql.IsSuccess)
            {
                return Result<List<BedroomStatisticsDto>>.Failure(sql.ErrorMessage);
            }

            try
            {
                var result = await connection.QueryAsync<BedroomStatisticsDto>(sql.Value);
                return Result<List<BedroomStatisticsDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<BedroomStatisticsDto>>.Failure(ex.Message);
            }
        }

        public async Task<Result<List<HostLargeApartmentDto>>> GetHostLargeAvarageApartament()
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            var sql = await LoadSql("HostsWithLargeAverageApartments.sql");
            if (!sql.IsSuccess)
            {
                return Result<List<HostLargeApartmentDto>>.Failure("File wasn`t found");
            }

            try
            {
                var result = await connection.QueryAsync<HostLargeApartmentDto>(sql.Value);
                return Result<List<HostLargeApartmentDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<HostLargeApartmentDto>>.Failure(ex.Message);
            }
        }

        public async Task<Result<decimal>> GetMedianArea()
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            var sql = await LoadSql("MedianArea.sql");
            if(!sql.IsSuccess)
            {
                return Result<decimal>.Failure(sql.ErrorMessage);
            }

            try
            {
                var result = await connection.QuerySingleAsync<decimal>(sql.Value);
                return Result<decimal>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<decimal>.Failure(ex.Message);
            } 
        }

        public async Task<Result<List<TotalAreaCountBySourceDto>>> GetTotalAreaCountBySourceCompany()
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            var sql = await LoadSql("TotalAreaAndCountBySourceCompany.sql");
            if (!sql.IsSuccess)
            {
                return Result<List<TotalAreaCountBySourceDto>>.Failure(sql.ErrorMessage);
            }

            try
            {
                var result = await connection.QueryAsync<TotalAreaCountBySourceDto>(sql.Value);
                return Result<List<TotalAreaCountBySourceDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<TotalAreaCountBySourceDto>>.Failure(ex.Message);
            }
        }

        private async Task<Result<string>> LoadSql(string fileName)
        {

            var path = Path.Combine(_sqlScriptsPath,fileName);
            if (!File.Exists(path))
            {
                return Result<string>.Failure("File wasn`t found");
            }

            try
            {
                return  Result<string>.Success(await File.ReadAllTextAsync(path));
            }
            catch (Exception ex)
            {
                return Result<string>.Failure(ex.Message);
            }
        }
    }
}
