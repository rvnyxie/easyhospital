using Refined.EasyHospital.Base;
using System;
using Volo.Abp.Application.Services;

namespace Refined.EasyHospital.Districts
{
    /// <summary>
    /// App service inteface for District
    /// </summary>
    public interface IDistrictAppService :
        ICrudAppService<DistrictDto, Guid, LocalityPagedAndSortedResultRequestDto, DistrictCreateDto, DistrictUpdateDto>,
        IBaseLocalityAppService
    {
    }
}
