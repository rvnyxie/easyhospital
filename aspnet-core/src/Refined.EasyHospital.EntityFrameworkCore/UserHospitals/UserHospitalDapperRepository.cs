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
        public async Task<List<UserHospital>> GetManyAsync(string? search, int pageSize, int currentPage)
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
                SELECT Id, UserId, HospitalId 
                FROM AppUserHospitals
                LIMIT @pageSize OFFSET @offset
                ";

            // Get data
            var userHospitals = await dbConnection.QueryAsync<UserHospital>(sqlCommand, parameters, transaction: await GetDbTransactionAsync());

            // Return result
            return userHospitals.ToList();
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
