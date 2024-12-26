using Dapper;
using Refined.EasyHospital.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.Dapper;
using Volo.Abp.EntityFrameworkCore;

namespace Refined.EasyHospital.Patients
{
    /// <summary>
    /// Dapper repository implementation for patient
    /// </summary>
    /// <param name="dbContextProvider">App Db context provider</param>
    public class PatientDapperRepository(
        IDbContextProvider<EasyHospitalDbContext> dbContextProvider)
        :
        DapperRepository<EasyHospitalDbContext>(dbContextProvider),
        IScopedDependency,
        IPatientDapperRepository
    {
        public Task<(List<Patient>, int)> GetManyAsync(string? search, int pageSize, int currentPage)
        {
            throw new NotImplementedException();
        }

        public async Task<(List<Patient>, int)> GetManyPatientsWithPaginationAndSearch(string? search, int pageSize, int currentPage, string? provinceCode, string? districtCode, string? communeCode, Guid hospitalId)
        {
            // Get connection
            var dbConnection = await GetDbConnectionAsync();

            // Validate sql parameters
            var parameters = new DynamicParameters();
            parameters.Add("search", $"%{search}%");
            parameters.Add("provinceCode", provinceCode);
            parameters.Add("districtCode", districtCode);
            parameters.Add("communeCode", communeCode);
            parameters.Add("hospitalId", hospitalId);
            parameters.Add("pageSize", pageSize);
            parameters.Add("offset", (currentPage - 1) * pageSize);

            // Compose sql command
            var countSqlCommand = @"
                SELECT COUNT(*)
                FROM AppPatients
                WHERE 
                    (@search IS NULL OR Code LIKE @search OR Name LIKE @search)
                    AND (@provinceCode IS NULL OR ProvinceCode = @provinceCode)
                    AND (@districtCode IS NULL OR DistrictCode = @districtCode)
                    AND (@communeCode IS NULL OR CommuneCode = @communeCode)
                    AND (HospitalId = @hospitalId)
                ";

            var dataSqlCommand = @"
                SELECT Id, Code, Name, ProvinceCode, DistrictCode, CommuneCode, HospitalId 
                FROM AppPatients
                WHERE 
                    (@search IS NULL OR Code LIKE @search OR Name LIKE @search)
                    AND (@provinceCode IS NULL OR ProvinceCode = @provinceCode)
                    AND (@districtCode IS NULL OR DistrictCode = @districtCode)
                    AND (@communeCode IS NULL OR CommuneCode = @communeCode)
                    AND (HospitalId = @hospitalId)
                ORDER BY Code ASC
                LIMIT @pageSize OFFSET @offset
                ";

            // Get data
            var totalCountWithSearch = await dbConnection.QuerySingleAsync<int>(countSqlCommand, parameters, transaction: await GetDbTransactionAsync());

            var patients = await dbConnection.QueryAsync<Patient>(dataSqlCommand, parameters, transaction: await GetDbTransactionAsync());

            // Return result
            return (patients.ToList(), totalCountWithSearch);
        }

        public Task<Patient> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Patient> GetPatientByIdAsync(Guid id, Guid hospitalId)
        {
            // Get connection
            var dbConnection = await GetDbConnectionAsync();

            // Validate sql parameters
            var parameters = new DynamicParameters();
            parameters.Add("id", id);
            parameters.Add("hospitalId", hospitalId);

            // Compose sql command
            var sqlCommand = @"
                SELECT Id, Code, Name, ProvinceCode, DistrictCode, CommuneCode, HospitalId 
                FROM AppPatients 
                WHERE (Id = @id AND HospitalId = @hospitalId)
            ";

            // Get data
            var patient = await dbConnection.QueryFirstOrDefaultAsync<Patient>(sqlCommand, parameters, transaction: await GetDbTransactionAsync());

            // Return result
            return patient;
        }
    }
}
