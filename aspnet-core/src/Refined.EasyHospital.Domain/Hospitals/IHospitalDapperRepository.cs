using Refined.EasyHospital.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Refined.EasyHospital.Hospitals
{
    /// <summary>
    /// Dapper repository interface for Hospital
    /// </summary>
    public interface IHospitalDapperRepository : IBaseDapperRepository<Hospital, Guid>
    {
        /// <summary>
        /// Get many hospitals with pagination and searches
        /// </summary>
        /// <param name="search">Search term</param>
        /// <param name="pageSize">Page size value</param>
        /// <param name="currentPage">Current page</param>
        /// <param name="provinceCode">Province Code</param>
        /// <param name="districtCode">District Code</param>
        /// <param name="communeCode">Commune Code</param>
        /// <returns></returns>
        Task<List<Hospital>> GetManyHospitalWithPaginationAndSearch(string? search, int pageSize, int currentPage, string? provinceCode, string? districtCode, string? communeCode);
    }
}
