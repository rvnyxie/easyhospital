using System;
using Volo.Abp.Application.Dtos;

namespace Refined.EasyHospital.Patients
{
    /// <summary>
    /// Patient DTO
    /// </summary>
    public class PatientDto : AuditedEntityDto<Guid>
    {
        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string ProvinceCode { get; set; } = string.Empty;

        public string DistrictCode { get; set; } = string.Empty;

        public string CommuneCode { get; set; } = string.Empty;

        public Guid HospitalId { get; set; }
    }
}
