using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Refined.EasyHospital.Provinces
{
    /// <summary>
    /// Service interface for Province
    /// </summary>
    public interface IProvinceService : ICrudAppService<ProvinceDto, Guid, PagedAndSortedResultRequestDto, ProvinceCreateDto, ProvinceUpdateDto>
    {
        /// <summary>
        /// Override asynchronous get by ID
        /// </summary>
        /// <param name="id">Province ID</param>
        /// <returns>Province DTO</returns>
        new Task<ProvinceDto> GetAsync(Guid id);
    }
}
