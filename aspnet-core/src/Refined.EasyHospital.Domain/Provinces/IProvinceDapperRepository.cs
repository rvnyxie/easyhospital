using Refined.EasyHospital.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Refined.EasyHospital.Provinces
{
    /// <summary>
    /// Dapper repository interface for Province
    /// </summary>
    public interface IProvinceDapperRepository : IBaseDapperRepository<Province, Guid>
    {
        /// <summary>
        /// Async get province by code
        /// </summary>
        /// <param name="code">Province code</param>
        /// <returns></returns>
        Task<Province> GetByCodeAsync(string code);

        /// <summary>
        /// Async get province by name
        /// </summary>
        /// <param name="name">Province name</param>
        /// <returns></returns>
        Task<Province> GetByNameAsync(string name);

        /// <summary>
        /// Async get provinces by names or codes
        /// </summary>
        /// <param name="codes">Province codes</param>
        /// <param name="names">Province names</param>
        /// <returns></returns>
        Task<List<Province>> GetByCodesOrNamesAsync(IEnumerable<string> codes, IEnumerable<string> names);
    }
}
