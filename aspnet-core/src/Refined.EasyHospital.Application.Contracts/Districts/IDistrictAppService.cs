using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Refined.EasyHospital.Districts
{
    /// <summary>
    /// App service inteface for District
    /// </summary>
    public interface IDistrictAppService : ICrudAppService<DistrictDto, Guid, PagedAndSortedResultRequestDto, DistrictCreateDto, DistrictUpdateDto>
    {
    }
}
