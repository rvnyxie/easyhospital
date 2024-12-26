using Refined.EasyHospital.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Refined.EasyHospital.Districts
{
    /// <summary>
    /// Dapper repository interface for District
    /// </summary>
    public interface IDistrictDapperRepository : IBaseLocalityDapperRepository<District, Guid>
    {
        /// <summary>
        /// Asynchronously get many districts by province code with pagination and search
        /// </summary>
        /// <param name="search">Search term</param>
        /// <param name="provinceCode">Province Code</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="currentPage">Current page</param>
        /// <returns></returns>
        Task<(List<District>, int)> GetManyByProvinceCodeAsync(string? search, string? provinceCode, int pageSize, int currentPage);
    }
}
