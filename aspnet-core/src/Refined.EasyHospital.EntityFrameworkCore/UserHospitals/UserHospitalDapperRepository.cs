using Dapper;
using Refined.EasyHospital.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.Dapper;
using Volo.Abp.EntityFrameworkCore;

namespace Refined.EasyHospital.UserHospitals
{
    /// <summary>
    /// Dapper repository implementation for user-hospital
    /// </summary>
    /// <param name="dbContextProvider"></param>
    public class UserHospitalDapperRepository(
        IDbContextProvider<EasyHospitalDbContext> dbContextProvider)
        :
        DapperRepository<EasyHospitalDbContext>(dbContextProvider),
        IScopedDependency,
        IUserHospitalDapperRepository
    {
        public async Task<(List<UserHospital>, int)> GetManyAsync(string? search, int pageSize, int currentPage)
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
                FROM AppUserHospitals
                ";

            var dataSqlCommand = @"
                SELECT uh.Id, uh.UserId, u.Name As UserName, HospitalId, h.Name as HospitalName
                FROM AppUserHospitals
                LIMIT @pageSize OFFSET @offset
                ";

            // Get data
            var totalCountWithSearch = await dbConnection.QuerySingleAsync<int>(countSqlCommand, parameters, transaction: await GetDbTransactionAsync());

            var userHospitals = await dbConnection.QueryAsync<UserHospital>(dataSqlCommand, parameters, transaction: await GetDbTransactionAsync());

            // Return result
            return (userHospitals.ToList(), totalCountWithSearch);
        }

        public async Task<(List<UserHospitalDto>, int)> GetManyDtosAsync(string? search, int pageSize, int currentPage)
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
                FROM AppUserHospitals uh
                INNER JOIN AbpUsers u ON uh.UserId = u.Id
                INNER JOIN AppHospitals h ON uh.HospitalId = h.Id
                WHERE @search IS NULL OR u.Name LIKE @search OR h.Name LIKE @search
                ";

            var dataSqlCommand = @"
                SELECT uh.Id, uh.UserId, u.Name As UserName, HospitalId, h.Name as HospitalName
                FROM AppUserHospitals uh
                INNER JOIN AbpUsers u ON uh.UserId = u.Id
                INNER JOIN AppHospitals h ON uh.HospitalId = h.Id
                WHERE @search IS NULL OR u.Name LIKE @search OR h.Name LIKE @search
                LIMIT @pageSize OFFSET @offset
                ";

            // Get data
            var totalCountWithSearch = await dbConnection.QuerySingleAsync<int>(countSqlCommand, parameters, transaction: await GetDbTransactionAsync());

            var userHospitals = await dbConnection.QueryAsync<UserHospitalDto>(dataSqlCommand, parameters, transaction: await GetDbTransactionAsync());

            // Return result
            return (userHospitals.ToList(), totalCountWithSearch);
        }

        public async Task<UserHospital> GetAsync(Guid id)
        {
            // Get connection
            var dbConnection = await GetDbConnectionAsync();

            // Validate sql parameters
            var parameters = new DynamicParameters();
            parameters.Add("id", id);

            // Compose sql command
            var sqlCommand = "SELECT Id, UserId, HospitalId FROM AppUserHospitals WHERE id = @id";

            // Get data
            var userHospital = await dbConnection.QueryFirstOrDefaultAsync<UserHospital>(sqlCommand, parameters, transaction: await GetDbTransactionAsync());

            // Return result
            return userHospital;
        }

        public async Task<UserHospital> GetByUserIdAndHospitalIdAsync(Guid userId, Guid hospitalId)
        {
            // Get connection
            var dbConnection = await GetDbConnectionAsync();

            // Validate sql parameters
            var parameters = new DynamicParameters();
            parameters.Add("userId", userId);
            parameters.Add("hospitalId", hospitalId);

            // Compose sql command
            var sqlCommand = @"
                SELECT Id, UserId, HospitalId FROM AppUserHospitals
                WHERE 
                    (@userId IS NULL OR UserID = @userID)
                    AND (@hospitalId IS NULL OR HospitalId = @hospitalId)
                ";

            // Get data
            var userHospital = await dbConnection.QueryFirstOrDefaultAsync<UserHospital>(sqlCommand, parameters, transaction: await GetDbTransactionAsync());

            // Return result
            return userHospital;
        }

        public async Task<UserHospital> GetByUserIdAsync(Guid userId)
        {
            // Get connection
            var dbConnection = await GetDbConnectionAsync();

            // Validate sql parameters
            var parameters = new DynamicParameters();
            parameters.Add("userId", userId);

            // Compose sql command
            var sqlCommand = @"
                SELECT Id, UserId, HospitalId FROM AppUserHospitals WHERE UserID = @userID";

            // Get data
            var userHospital = await dbConnection.QueryFirstOrDefaultAsync<UserHospital>(sqlCommand, parameters, transaction: await GetDbTransactionAsync());

            // Return result
            return userHospital;
        }
    }
}
