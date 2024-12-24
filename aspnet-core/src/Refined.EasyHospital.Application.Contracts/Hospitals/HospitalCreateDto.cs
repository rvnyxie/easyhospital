using System.ComponentModel.DataAnnotations;

namespace Refined.EasyHospital.Hospitals
{
    /// <summary>
    /// Creation DTO for Hospital
    /// </summary>
    public class HospitalCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string ProvinceCode { get; set; } = string.Empty;

        [Required]
        public string DistrictCode { get; set; } = string.Empty;

        [Required]
        public string CommuneCode { get; set; } = string.Empty;
    }
}
