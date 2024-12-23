using Dapper;
using Refined.EasyHospital.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.Dapper;
using Volo.Abp.EntityFrameworkCore;

namespace Refined.EasyHospital.Provinces
{
    /// <summary>
    /// Dapper repository implementation for Province
    /// </summary>
    /// <param name="dbContextProvider"></param>
    public class ProvinceDapperRepository(IDbContextProvider<EasyHospitalDbContext> dbContextProvider) :
        DapperRepository<EasyHospitalDbContext>(dbContextProvider),
        ITransientDependency,
        IProvinceDapperRepository
    {
        public async Task<List<Province>> GetManyAsync(string? search, int pageSize, int currentPage)
        {
            // Get connection
            var dbConnection = await GetDbConnectionAsync();

            // Validate sql parameters
            var parameters = new DynamicParameters();
            parameters.Add("search", $"%{search}%");
            parameters.Add("pageSize", pageSize);
            parameters.Add("offset", (currentPage - 1) * pageSize);

            // Compose sql command
            var sqlCommand = @"
                SELECT Id, Code, Name, EnglishName, DecisionDate, EffectiveDate, Description, Level, Population, Area 
                FROM AppProvinces
                WHERE (@search IS NULL OR Code LIKE @search OR Name LIKE @search)
                ORDER BY Code ASC
                LIMIT @pageSize OFFSET @offset
                ";

            // Get data
            var provinces = await dbConnection.QueryAsync<Province>(sqlCommand, parameters, transaction: await GetDbTransactionAsync());

            // Return result
            return provinces.ToList();
        }

        public async Task<Province> GetAsync(Guid id)
        {
            // Get connection
            var dbConnection = await GetDbConnectionAsync();

            // Validate sql parameters
            var parameters = new DynamicParameters();
            parameters.Add("id", id);

            // Compose sql command
            var sqlCommand = "SELECT Id, Code, Name, EnglishName, DecisionDate, EffectiveDate, Description, Level, Population, Area FROM AppProvinces WHERE id = @id";

            // Get data
            var province = await dbConnection.QueryFirstOrDefaultAsync<Province>(sqlCommand, parameters, transaction: await GetDbTransactionAsync());

            // Return result
            return province;
        }

        public async Task<Province> GetByCodeAsync(string code)
        {
            // Get connection
            var dbConnection = await GetDbConnectionAsync();

            // Validate sql parameters
            var parameters = new DynamicParameters();
            parameters.Add("code", code);

            // Compose sql command
            var sqlCommand = "SELECT Id, Code, Name, EnglishName, DecisionDate, EffectiveDate, Description, Level, Population, Area FROM AppProvinces WHERE code = @code";

            // Get data
            var province = await dbConnection.QueryFirstOrDefaultAsync<Province>(sqlCommand, parameters, transaction: await GetDbTransactionAsync());

            // Return result
            return province;
        }

        public async Task<Province> GetByNameAsync(string name)
        {
            // Get connection
            var dbConnection = await GetDbConnectionAsync();

            // Validate sql parameters
            var parameters = new DynamicParameters();
            parameters.Add("name", name);

            // Compose sql command
            var sqlCommand = "SELECT Id, Code, Name, EnglishName, DecisionDate, EffectiveDate, Description, Level, Population, Area FROM AppProvinces WHERE name = @name";

            // Get data
            var province = await dbConnection.QueryFirstOrDefaultAsync<Province>(sqlCommand, parameters, transaction: await GetDbTransactionAsync());

            // Return result
            return province;
        }

        public async Task<List<Province>> GetByCodesOrNamesAsync(IEnumerable<string> codes, IEnumerable<string> names)
        {
            // Get connection
            var dbConnection = await GetDbConnectionAsync();

            // Validate sql parameters
            var parameters = new DynamicParameters();
            parameters.Add("codes", codes);
            parameters.Add("names", names);

            // Compose sql command
            var sqlCommand = @"
                SELECT Id, Code, Name, EnglishName, DecisionDate, EffectiveDate, Description, Level, Population, Area 
                FROM AppProvinces
                WHERE (Code in @codes OR Name in @names)
                ORDER BY Code ASC
                ";

            // Get data
            var provinces = await dbConnection.QueryAsync<Province>(sqlCommand, parameters, transaction: await GetDbTransactionAsync());

            // Return result
            return provinces.ToList();
        }
    }
}
