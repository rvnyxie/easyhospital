using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Refined.EasyHospital.Patients
{
    /// <summary>
    /// Patient entity
    /// </summary>
    public class Patient : AuditedAggregateRoot<Guid>
    {
        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string ProvinceCode { get; set; } = string.Empty;

        public string DistrictCode { get; set; } = string.Empty;

        public string CommuneCode { get; set; } = string.Empty;

        public Guid HospitalId { get; set; }
    }
}
