using Dapper;
using Refined.EasyHospital.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.Dapper;
using Volo.Abp.EntityFrameworkCore;

namespace Refined.EasyHospital.Districts
{
    /// <summary>
    /// Dapper repository implementation for District
    /// </summary>
    public class DistrictDapperRepository(
        IDbContextProvider<EasyHospitalDbContext> dbContextProvider)
        :
        DapperRepository<EasyHospitalDbContext>(dbContextProvider),
        ITransientDependency,
        IDistrictDapperRepository
    {
        public async Task<List<District>> GetManyAsync(string? search, int pageSize, int currentPage)
        {
            // Get connection
            using (var dbConnection = await GetDbConnectionAsync())
            {
                // Validate sql parameters
                var parameters = new DynamicParameters();
                parameters.Add("search", $"%{search}%");
                parameters.Add("pageSize", pageSize);
                parameters.Add("offset", (currentPage - 1) * pageSize);

                // Compose sql command
                //var sqlCommand = "SELECT Id, Code, Name, EnglishName, DecisionDate, EffectiveDate, Description, Level, Population, Area, ProvinceId FROM AppDistricts";
                var sqlCommand = @"
                    SELECT Id, Code, Name, EnglishName, DecisionDate, EffectiveDate, Description, Level, Population, Area 
                    FROM AppDistricts
                    WHERE (@search IS NULL OR Code LIKE @search OR Name LIKE @search)
                    ORDER BY Code ASC
                    LIMIT @pageSize OFFSET @offset
                    ";

                // Get data
                var districts = await dbConnection.QueryAsync<District>(sqlCommand, parameters, transaction: await GetDbTransactionAsync());

                // Return result
                return districts.ToList();
            }
        }

        public async Task<District> GetAsync(Guid id)
        {
            // Get connection
            using (var dbConnection = await GetDbConnectionAsync())
            {
                // Validate sql parameters
                var parameters = new DynamicParameters();
                parameters.Add("id", id, System.Data.DbType.Guid);

                // Compose sql command
                var sqlCommand = "SELECT Id, Code, Name, EnglishName, DecisionDate, EffectiveDate, Description, Level, Population, Area, ProvinceId FROM AppDistricts WHERE id = @id";

                // Get data
                var district = await dbConnection.QueryFirstOrDefaultAsync<District>(sqlCommand, parameters, transaction: await GetDbTransactionAsync());

                // Return result
                return district;
            }
        }
    }
}
