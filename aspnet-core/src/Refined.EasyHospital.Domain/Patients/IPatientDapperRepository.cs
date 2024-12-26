using Refined.EasyHospital.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Refined.EasyHospital.Patients
{
    /// <summary>
    /// Dapper repository interface for patient
    /// </summary>
    public interface IPatientDapperRepository : IBaseDapperRepository<Patient, Guid>
    {
        /// <summary>
        /// Asynchronously get many patients with pagination and searches
        /// </summary>
        /// <param name="search">Search term</param>
        /// <param name="pageSize">Page size value</param>
        /// <param name="currentPage">Current page</param>
        /// <param name="provinceCode">Province Code</param>
        /// <param name="districtCode">District Code</param>
        /// <param name="communeCode">Commune Code</param>
        /// <param name="hospitalId">Hospital ID</param>
        /// <returns>List of patients and total quantity of patient</returns>
        Task<(List<Patient>, int)> GetManyPatientsWithPaginationAndSearch(string? search, int pageSize, int currentPage, string? provinceCode, string? districtCode, string? communeCode, Guid hospitalId);

        /// <summary>
        /// Asynchronously get patient by ID
        /// </summary>
        /// <param name="id">Patient ID</param>
        /// <param name="hospitalId">Hospital ID</param>
        /// <returns></returns>
        Task<Patient> GetPatientByIdAsync(Guid id, Guid hospitalId);
    }
}
