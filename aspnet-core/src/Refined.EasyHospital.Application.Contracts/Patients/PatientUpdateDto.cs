using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Refined.EasyHospital.Patients
{
    /// <summary>
    /// Update DTO for patient
    /// </summary>
    public class PatientUpdateDto : EntityDto<Guid>
    {
        [Required]
        public string Code { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string ProvinceCode { get; set; } = string.Empty;

        [Required]
        public string DistrictCode { get; set; } = string.Empty;

        [Required]
        public string CommuneCode { get; set; } = string.Empty;

        public Guid HospitalId { get; set; }
    }
}
