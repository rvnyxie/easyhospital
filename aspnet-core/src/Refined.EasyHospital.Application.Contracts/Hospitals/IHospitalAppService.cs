using System;
using Volo.Abp.Application.Services;

namespace Refined.EasyHospital.Hospitals
{
    /// <summary>
    /// App service interface for Hospital
    /// </summary>
    public interface IHospitalAppService : ICrudAppService<HospitalDto, Guid, HospitalPagedAndSortedResultRequestDto, HospitalCreateDto, HospitalUpdateDto>
    {
    }
}
