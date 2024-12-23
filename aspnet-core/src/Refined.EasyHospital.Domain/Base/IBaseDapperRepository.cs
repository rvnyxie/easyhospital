using System.Collections.Generic;
using System.Threading.Tasks;

namespace Refined.EasyHospital.Base
{
    /// <summary>
    /// Base Dapper repository
    /// </summary>
    public interface IBaseDapperRepository<TEntity, TKey>
    {
        /// <summary>
        /// Async get many entities
        /// </summary>
        /// <returns></returns>
        Task<List<TEntity>> GetManyAsync(string? search, int pageSize, int currentPage);

        /// <summary>
        /// Async get entity by ID
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <returns></returns>
        Task<TEntity> GetAsync(TKey id);
    }
}
