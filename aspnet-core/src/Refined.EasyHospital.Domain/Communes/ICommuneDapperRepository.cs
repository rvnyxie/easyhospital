using Refined.EasyHospital.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Refined.EasyHospital.Communes
{
    /// <summary>
    /// Dapper repository interface for Commune
    /// </summary>
    public interface ICommuneDapperRepository : IBaseLocalityDapperRepository<Commune, Guid>
    {
        /// <summary>
        /// Asynchronously get many communes by districtCode code with pagination and search
        /// </summary>
        /// <param name="search">Search term</param>
        /// <param name="districtCode">District Code</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="currentPage">Current page</param>
        /// <returns></returns>
        Task<(List<Commune>, int)> GetManyByDistrictCodeAsync(string? search, string? districtCode, int pageSize, int currentPage);
    }
}
