using System;
using Volo.Abp.Application.Services;

namespace Refined.EasyHospital.Patients
{
    /// <summary>
    /// App service interface for patient
    /// </summary>
    public interface IPatientAppService : ICrudAppService<PatientDto, Guid, PatientPagedAndSortedResultRequestDto, PatientCreateDto, PatientUpdateDto>
    {
    }
}
