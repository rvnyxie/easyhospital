using Refined.EasyHospital.Provinces;
using System;
using System.ComponentModel.DataAnnotations;

namespace Refined.EasyHospital.Districts
{
    /// <summary>
    /// Update DTO for District
    /// </summary>
    public class DistrictUpdateDto
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(3)]
        public string Code { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public string? EnglishName { get; set; }

        public DateTime? DecisionDate { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public int Population { get; set; }

        public float Area { get; set; }

        [MaxLength(512)]
        public string? Description { get; set; }

        public ProvinceLevel Level { get; set; }

        public string ProvinceCode { get; set; } = string.Empty;
    }
}
