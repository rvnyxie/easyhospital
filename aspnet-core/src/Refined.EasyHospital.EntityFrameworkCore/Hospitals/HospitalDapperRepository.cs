using Dapper;
using Refined.EasyHospital.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.Dapper;
using Volo.Abp.EntityFrameworkCore;

namespace Refined.EasyHospital.Hospitals
{
    /// <summary>
    /// Dapper repository implementation for Hospital
    /// </summary>
    /// <param name="dbContextProvider"></param>
    public class HospitalDapperRepository(
        IDbContextProvider<EasyHospitalDbContext> dbContextProvider)
        :
        DapperRepository<EasyHospitalDbContext>(dbContextProvider),
        IScopedDependency,
        IHospitalDapperRepository
    {
        public async Task<List<Hospital>> GetManyAsync(string? search, int pageSize, int currentPage)
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
                SELECT Id, Name, ProvinceCode, DistrictCode, CommuneCode 
                FROM AppHospitals
                WHERE (@search IS NULL OR Name LIKE @search)
                ORDER BY Name ASC
                LIMIT @pageSize OFFSET @offset
                ";

            // Get data
            var hospitals = await dbConnection.QueryAsync<Hospital>(sqlCommand, parameters, transaction: await GetDbTransactionAsync());

            // Return result
            return hospitals.ToList();
        }

        public async Task<Hospital> GetAsync(Guid id)
        {
            // Get connection
            var dbConnection = await GetDbConnectionAsync();

            // Validate sql parameters
            var parameters = new DynamicParameters();
            parameters.Add("id", id, System.Data.DbType.Guid);

            // Compose sql command
            var sqlCommand = "SELECT Id, Name, ProvinceCode, DistrictCode, CommuneCode FROM AppHospitals WHERE id = @id";

            // Get data
            var hospital = await dbConnection.QueryFirstOrDefaultAsync<Hospital>(sqlCommand, parameters, transaction: await GetDbTransactionAsync());

            // Return result
            return hospital;
        }
    }
}
