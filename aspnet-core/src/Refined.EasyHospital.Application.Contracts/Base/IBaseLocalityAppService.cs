using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Content;

namespace Refined.EasyHospital.Base
{
    /// <summary>
    /// Base app service interface for Locality
    /// </summary>
    public interface IBaseLocalityAppService
    {
        /// <summary>
        /// Import locality data from excel file
        /// </summary>
        /// <param name="input">Excel file stream content</param>
        /// <returns>List of errors, empty if successful</returns>
        Task<List<string>> Import(IRemoteStreamContent input);
    }
}
