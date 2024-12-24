using Refined.EasyHospital.Base;
using System;
using Volo.Abp.Application.Services;

namespace Refined.EasyHospital.Hospitals
{
    /// <summary>
    /// App service interface for Hospital
    /// </summary>
    public interface IHospitalAppService : ICrudAppService<HospitalDto, Guid, ExtendedPagedAndSortedResultRequestDto, HospitalCreateDto, HospitalUpdateDto>
    {
    }
}
