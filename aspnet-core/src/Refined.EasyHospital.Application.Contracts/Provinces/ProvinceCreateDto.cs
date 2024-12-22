using System;
using System.ComponentModel.DataAnnotations;

namespace Refined.EasyHospital.Provinces
{
    /// <summary>
    /// Creation DTO for Province
    /// </summary>
    public class ProvinceCreateDto
    {
        [Required]
        [MaxLength(2)]
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
    }
}
