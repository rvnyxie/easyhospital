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
        public async Task<List<District>> GetManyAsync()
        {
            // Get connection
            using (var dbConnection = await GetDbConnectionAsync())
            {
                // Validate sql parameters

                // Compose sql command
                var sqlCommand = "SELECT Id, Code, Name, EnglishName, DecisionDate, EffectiveDate, Description, Level, Population, Area, ProvinceId FROM AppDistricts";

                // Get data
                var districts = await dbConnection.QueryAsync<District>(sqlCommand, transaction: await GetDbTransactionAsync());

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
                var districts = await dbConnection.QueryFirstOrDefaultAsync<District>(sqlCommand, parameters, transaction: await GetDbTransactionAsync());

                // Return result
                return districts;
            }
        }
    }
}
