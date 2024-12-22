using Dapper;
using Refined.EasyHospital.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.Dapper;
using Volo.Abp.EntityFrameworkCore;

namespace Refined.EasyHospital.Communes
{
    /// <summary>
    /// Dapper repository implementation for Commune
    /// </summary>
    /// <param name="dbContextProvider">App DB Context</param>
    public class CommuneDapperRepository(
        IDbContextProvider<EasyHospitalDbContext> dbContextProvider)
        :
        DapperRepository<EasyHospitalDbContext>(dbContextProvider),
        ITransientDependency,
        ICommuneDapperRepository
    {
        public async Task<List<Commune>> GetManyAsync()
        {
            // Get connection
            using (var dbConnection = await GetDbConnectionAsync())
            {
                // Validate sql parameters

                // Compose sql command
                var sqlCommand = "SELECT Id, Code, Name, EnglishName, DecisionDate, EffectiveDate, Description, Level, Population, Area, DistrictId FROM AppCommunes";

                // Get data
                var communes = await dbConnection.QueryAsync<Commune>(sqlCommand, transaction: await GetDbTransactionAsync());

                // Return result
                return communes.ToList();
            }
        }

        public async Task<Commune> GetAsync(Guid id)
        {
            using (var dbConnection = await GetDbConnectionAsync())
            {
                // Validate sql parameters
                var parameters = new DynamicParameters();
                parameters.Add("id", id, System.Data.DbType.Guid);

                // Compose sql command
                var sqlCommand = "SELECT Id, Code, Name, EnglishName, DecisionDate, EffectiveDate, Description, Level, Population, Area, DistrictId FROM AppCommunes WHERE id = @id";

                // Get data
                var commune = await dbConnection.QueryFirstOrDefaultAsync<Commune>(sqlCommand, parameters, transaction: await GetDbTransactionAsync());

                // Return result
                return commune;
            }
        }
    }
}
