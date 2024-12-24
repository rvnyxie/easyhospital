using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Refined.EasyHospital.Hospitals
{
    /// <summary>
    /// Hospital entity
    /// </summary>
    public class Hospital : AuditedAggregateRoot<Guid>
    {
        public string Name { get; set; } = string.Empty;

        public string? Address { get; set; }

        public string ProvinceCode { get; set; } = string.Empty;

        public string DistrictCode { get; set; } = string.Empty;

        public string CommuneCode { get; set; } = string.Empty;
    }
}
