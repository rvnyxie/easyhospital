using Refined.EasyHospital.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace Refined.EasyHospital.Provinces
{
    /// <summary>
    /// Service interface for Province
    /// </summary>
    public interface IProvinceAppService : ICrudAppService<ProvinceDto, Guid, LocalityPagedAndSortedResultRequestDto, ProvinceCreateDto, ProvinceUpdateDto>
    {
        /// <summary>
        /// Import province data from excel file
        /// </summary>
        /// <param name="input">Excel file stream content</param>
        /// <returns>List of errors, empty if successful</returns>
        Task<List<string>> Import(IRemoteStreamContent input);
    }
}
