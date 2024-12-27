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
        public async Task<(List<Hospital>, int)> GetManyAsync(string? search, int pageSize, int currentPage)
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
                FROM AppHospitals
                WHERE (@search IS NULL OR Name LIKE @search OR Address LIKE @search)
                ";

            var dataSqlCommand = @"
                SELECT Id, Name, Address, ProvinceCode, DistrictCode, CommuneCode 
                FROM AppHospitals
                WHERE (@search IS NULL OR Name LIKE @search OR Address LIKE @search)
                ORDER BY Name ASC
                LIMIT @pageSize OFFSET @offset
                ";

            // Get data
            var totalCountWithSearch = await dbConnection.QuerySingleAsync<int>(countSqlCommand, parameters, transaction: await GetDbTransactionAsync());

            var hospitals = await dbConnection.QueryAsync<Hospital>(dataSqlCommand, parameters, transaction: await GetDbTransactionAsync());

            // Return result
            return (hospitals.ToList(), totalCountWithSearch);
        }

        public async Task<(List<Hospital>, int)> GetManyHospitalWithPaginationAndSearch(string? search, int pageSize, int currentPage, string? provinceCode, string? districtCode, string? communeCode)
        {
            // Get connection
            var dbConnection = await GetDbConnectionAsync();

            // Validate sql parameters
            var parameters = new DynamicParameters();
            parameters.Add("search", $"%{search}%");
            parameters.Add("provinceCode", provinceCode);
            parameters.Add("districtCode", districtCode);
            parameters.Add("communeCode", communeCode);
            parameters.Add("pageSize", pageSize);
            parameters.Add("offset", (currentPage - 1) * pageSize);

            // Compose sql command
            var countSqlCommand = @"
                SELECT COUNT(*)
                FROM AppHospitals
                WHERE 
                    (@search IS NULL OR Name LIKE @search OR Address LIKE @search)
                    AND (@provinceCode IS NULL OR ProvinceCode = @provinceCode)
                    AND (@districtCode IS NULL OR DistrictCode = @districtCode)
                    AND (@communeCode IS NULL OR CommuneCode = @communeCode)
                ";

            var dataSqlCommand = @"
                SELECT Id, Name, Address, ProvinceCode, DistrictCode, CommuneCode 
                FROM AppHospitals
                WHERE 
                    (@search IS NULL OR Name LIKE @search OR Address LIKE @search)
                    AND (@provinceCode IS NULL OR ProvinceCode = @provinceCode)
                    AND (@districtCode IS NULL OR DistrictCode = @districtCode)
                    AND (@communeCode IS NULL OR CommuneCode = @communeCode)
                ORDER BY Name ASC
                LIMIT @pageSize OFFSET @offset
                ";

            // Get data
            var totalCountWithSearch = await dbConnection.QuerySingleAsync<int>(countSqlCommand, parameters, transaction: await GetDbTransactionAsync());

            var hospitals = await dbConnection.QueryAsync<Hospital>(dataSqlCommand, parameters, transaction: await GetDbTransactionAsync());

            // Return result
            return (hospitals.ToList(), totalCountWithSearch);
        }

        public async Task<(List<HospitalDto>, int)> GetManyDtosWithPaginationAndSearchAsync(string? search, int pageSize, int currentPage, string? provinceCode, string? districtCode, string? communeCode)
        {
            // Get connection
            var dbConnection = await GetDbConnectionAsync();

            // Validate sql parameters
            var parameters = new DynamicParameters();
            parameters.Add("search", $"%{search}%");
            parameters.Add("provinceCode", provinceCode);
            parameters.Add("districtCode", districtCode);
            parameters.Add("communeCode", communeCode);
            parameters.Add("pageSize", pageSize);
            parameters.Add("offset", (currentPage - 1) * pageSize);

            // Compose sql command
            var countSqlCommand = @"
                SELECT COUNT(*)
                FROM AppHospitals
                WHERE 
                    (@search IS NULL OR Name LIKE @search OR Address LIKE @search)
                    AND (@provinceCode IS NULL OR ProvinceCode = @provinceCode)
                    AND (@districtCode IS NULL OR DistrictCode = @districtCode)
                    AND (@communeCode IS NULL OR CommuneCode = @communeCode)
                ";

            var dataSqlCommand = @"
                SELECT h.Id, h.Name, h.Address, h.ProvinceCode, h.DistrictCode, h.CommuneCode, p.Name as ProvinceName, d.Name as DistrictName, c.Name As CommuneName 
                FROM AppHospitals h
                INNER JOIN AppProvinces p ON h.ProvinceCode = p.Code
                INNER JOIN AppDistricts d ON h.DistrictCode = d.Code
                INNER JOIN AppCommunes c ON h.CommuneCode = c.Code
                WHERE 
                    (@search IS NULL OR h.Name LIKE @search OR h.Address LIKE @search)
                    AND (@provinceCode IS NULL OR h.ProvinceCode = @provinceCode)
                    AND (@districtCode IS NULL OR h.DistrictCode = @districtCode)
                    AND (@communeCode IS NULL OR h.CommuneCode = @communeCode)
                ORDER BY h.Name ASC
                LIMIT @pageSize OFFSET @offset
                ";

            // Get data
            var totalCountWithSearch = await dbConnection.QuerySingleAsync<int>(countSqlCommand, parameters, transaction: await GetDbTransactionAsync());

            var hospitals = await dbConnection.QueryAsync<HospitalDto>(dataSqlCommand, parameters, transaction: await GetDbTransactionAsync());

            // Return result
            return (hospitals.ToList(), totalCountWithSearch);
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
