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
        public async Task<(List<Commune>, int)> GetManyAsync(string? search, int pageSize, int currentPage)
        {
            // Get connection
            var dbConnection = await GetDbConnectionAsync();

            // Validate sql parameters
            var parameters = new DynamicParameters();
            parameters.Add("search", $"%{search}%");
            parameters.Add("pageSize", pageSize);
            parameters.Add("offset", (currentPage - 1) * pageSize);

            // Compose sql command
            var countSqlCommand = @"
                SELECT COUNT(*)
                FROM AppCommunes
                WHERE (@search IS NULL OR Code LIKE @search OR Name LIKE @search)
                ";

            var dataSqlCommand = @"
                SELECT Id, Code, Name, EnglishName, DecisionDate, EffectiveDate, Description, Level, Population, Area, DistrictCode 
                FROM AppCommunes
                WHERE (@search IS NULL OR Code LIKE @search OR Name LIKE @search)
                ORDER BY Code ASC
                LIMIT @pageSize OFFSET @offset
                ";

            // Get data
            var totalCountWithSearch = await dbConnection.QuerySingleAsync<int>(countSqlCommand, parameters, transaction: await GetDbTransactionAsync());

            var communes = await dbConnection.QueryAsync<Commune>(dataSqlCommand, parameters, transaction: await GetDbTransactionAsync());

            // Return result
            return (communes.ToList(), totalCountWithSearch);
        }

        public async Task<(List<Commune>, int)> GetManyByDistrictCodeAsync(string? search, string? districtCode, int pageSize, int currentPage)
        {
            // Get connection
            var dbConnection = await GetDbConnectionAsync();

            // Validate sql parameters
            var parameters = new DynamicParameters();
            parameters.Add("search", $"%{search}%");
            parameters.Add("districtCode", districtCode);
            parameters.Add("pageSize", pageSize);
            parameters.Add("offset", (currentPage - 1) * pageSize);

            // Compose sql command
            var countSqlCommand = @"
                SELECT COUNT(*)
                FROM AppCommunes
                WHERE 
                    (@search IS NULL OR Code LIKE @search OR Name LIKE @search)
                    AND (DistrictCode = @districtCode)
                ";

            var dataSqlCommand = @"
                SELECT Id, Code, Name, EnglishName, DecisionDate, EffectiveDate, Description, Level, Population, Area, DistrictCode 
                FROM AppCommunes
                WHERE 
                    (@search IS NULL OR Code LIKE @search OR Name LIKE @search)
                    AND (DistrictCode = @districtCode)
                ORDER BY Code ASC
                LIMIT @pageSize OFFSET @offset
                ";

            // Get data
            var totalCountWithSearch = await dbConnection.QuerySingleAsync<int>(countSqlCommand, parameters, transaction: await GetDbTransactionAsync());

            var communes = await dbConnection.QueryAsync<Commune>(dataSqlCommand, parameters, transaction: await GetDbTransactionAsync());

            // Return result
            return (communes.ToList(), totalCountWithSearch);
        }

        public async Task<Commune> GetAsync(Guid id)
        {
            var dbConnection = await GetDbConnectionAsync();

            // Validate sql parameters
            var parameters = new DynamicParameters();
            parameters.Add("id", id, System.Data.DbType.Guid);

            // Compose sql command
            var sqlCommand = "SELECT Id, Code, Name, EnglishName, DecisionDate, EffectiveDate, Description, Level, Population, Area, DistrictCode FROM AppCommunes WHERE id = @id";

            // Get data
            var commune = await dbConnection.QueryFirstOrDefaultAsync<Commune>(sqlCommand, parameters, transaction: await GetDbTransactionAsync());

            // Return result
            return commune;

        }

        public async Task<Commune> GetByCodeAsync(string code)
        {
            // Get connection
            var dbConnection = await GetDbConnectionAsync();

            // Validate sql parameters
            var parameters = new DynamicParameters();
            parameters.Add("code", code);

            // Compose sql command
            var sqlCommand = "SELECT Id, Code, Name, EnglishName, DecisionDate, EffectiveDate, Description, Level, Population, Area, DistrictCode FROM AppCommunes WHERE code = @code";

            // Get data
            var commune = await dbConnection.QueryFirstOrDefaultAsync<Commune>(sqlCommand, parameters, transaction: await GetDbTransactionAsync());

            // Return result
            return commune;
        }

        public async Task<Commune> GetByNameAsync(string name)
        {
            // Get connection
            var dbConnection = await GetDbConnectionAsync();

            // Validate sql parameters
            var parameters = new DynamicParameters();
            parameters.Add("name", name);

            // Compose sql command
            var sqlCommand = "SELECT Id, Code, Name, EnglishName, DecisionDate, EffectiveDate, Description, Level, Population, Area, DistrictCode FROM AppCommunes WHERE name = @name";

            // Get data
            var commune = await dbConnection.QueryFirstOrDefaultAsync<Commune>(sqlCommand, parameters, transaction: await GetDbTransactionAsync());

            // Return result
            return commune;
        }

        public async Task<List<Commune>> GetByCodesOrNamesAsync(IEnumerable<string> codes, IEnumerable<string> names)
        {
            // Get connection
            var dbConnection = await GetDbConnectionAsync();

            // Validate sql parameters
            var parameters = new DynamicParameters();
            parameters.Add("codes", codes);
            parameters.Add("names", names);

            // Compose sql command
            var sqlCommand = @"
                SELECT Id, Code, Name, EnglishName, DecisionDate, EffectiveDate, Description, Level, Population, Area, DistrictCode
                FROM AppCommunes
                WHERE (Code in @codes OR Name in @names)
                ORDER BY Code ASC
                ";

            // Get data
            var communes = await dbConnection.QueryAsync<Commune>(sqlCommand, parameters, transaction: await GetDbTransactionAsync());

            // Return result
            return communes.ToList();
        }
    }
}
