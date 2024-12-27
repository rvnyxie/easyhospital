using System;
using Volo.Abp.Application.Dtos;

namespace Refined.EasyHospital.Hospitals
{
    /// <summary>
    /// Hospital DTO
    /// </summary>
    public class HospitalDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; } = string.Empty;

        public string ProvinceCode { get; set; } = string.Empty;

        public string ProvinceName { get; set; } = string.Empty;

        public string DistrictCode { get; set; } = string.Empty;

        public string DistrictName { get; set; } = string.Empty;

        public string CommuneCode { get; set; } = string.Empty;

        public string CommuneName { get; set; } = string.Empty;
    }
}
