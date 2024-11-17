using BookingWebApi.Application.Apartament;
using BookingWebApi.Application.Apartament.Statistics.StatisticDTOs;
using BookingWebApi.Application.Common.Models;
using BookingWebApi.Application.Common.Response;
using BookingWebApi.Domain.Entities;
using BookingWebApi.Infrastructure.Configuration;
using BookingWebApi.Infrastructure.SqlScripts;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Resources;




namespace BookingWebApi.Infrastructure.Data
{
    public class ApartamentRepository : IApartamentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly string _connectionString;
        private static readonly Dictionary<string, string> _sqlCashe = new();

        public ApartamentRepository(ApplicationDbContext context, string connectionString)
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

            try
            {
                var sql = GetSqlQuery("AreaQuantiles", SqlFilePath.StatisticApartmentScripts);

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

            try
            {
                var sql = GetSqlQuery("AverageAreaByBedrooms", SqlFilePath.StatisticApartmentScripts);

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

            try
            {
                var sql = GetSqlQuery("HostsWithLargeAverageApartments", SqlFilePath.StatisticApartmentScripts);

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

            try
            {
                var sql = GetSqlQuery("MedianArea", SqlFilePath.StatisticApartmentScripts);

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

            try
            {
                var sql = GetSqlQuery("TotalAreaAndCountBySourceCompany", SqlFilePath.StatisticApartmentScripts);

                var result = await connection.QueryAsync<TotalAreaCountBySourceDto>(sql);
                return Result<List<TotalAreaCountBySourceDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<TotalAreaCountBySourceDto>>.Failure(ex.Message);
            }
        }

        public async Task<Result<bool>> UpsertCustomData(int apartamentId, string customData)
        {
            await using var connection = _context.Database.GetDbConnection();

            try
            {
                var sql = GetSqlQuery("Upsert", SqlFilePath.UpsertApartamentCustomDataScript);

                var result = await connection.ExecuteAsync(sql,new {ApartamentId = apartamentId, CustomData = customData});
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
        private string GetSqlQuery(string resourceName,string sqlPath)
        {
            if(_sqlCashe.TryGetValue(resourceName, out var cashedSql))
            {
                return cashedSql;
            }

            var resourceManager = new ResourceManager(sqlPath, typeof(ApartamentRepository).Assembly);
            var sql = resourceManager.GetString(resourceName);
            if(string.IsNullOrEmpty(sql))
            {
                throw new Exception($"SQL query {resourceName} not found");
            }

            _sqlCashe[resourceName] = sql;
            return sql;
        }
    }
}


