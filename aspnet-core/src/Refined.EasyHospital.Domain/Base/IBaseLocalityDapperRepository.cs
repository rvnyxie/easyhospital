using System.Collections.Generic;
using System.Threading.Tasks;

namespace Refined.EasyHospital.Base
{
    /// <summary>
    /// Base Dapper repository interface for Locality
    /// </summary>
    public interface IBaseLocalityDapperRepository<TEntity, TKey> : IBaseDapperRepository<TEntity, TKey>
    {
        /// <summary>
        /// Async get locality by code
        /// </summary>
        /// <param name="code">Locality code</param>
        /// <returns></returns>
        Task<TEntity> GetByCodeAsync(string code);

        /// <summary>
        /// Async get locality by name
        /// </summary>
        /// <param name="name">Locality name</param>
        /// <returns></returns>
        Task<TEntity> GetByNameAsync(string name);

        /// <summary>
        /// Async get many localities by names or codes
        /// </summary>
        /// <param name="codes">Locality codes</param>
        /// <param name="names">Locality names</param>
        /// <returns></returns>
        Task<List<TEntity>> GetByCodesOrNamesAsync(IEnumerable<string> codes, IEnumerable<string> names);
    }
}
