using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Refined.EasyHospital.Provinces
{
    /// <summary>
    /// Service interface for Province
    /// </summary>
    public interface IProvinceAppService : ICrudAppService<ProvinceDto, Guid, PagedAndSortedResultRequestDto, ProvinceCreateDto, ProvinceUpdateDto>
    {
    }
}
