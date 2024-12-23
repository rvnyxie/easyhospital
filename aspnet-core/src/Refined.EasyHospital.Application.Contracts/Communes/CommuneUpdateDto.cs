using System;
using System.ComponentModel.DataAnnotations;

namespace Refined.EasyHospital.Communes
{
    /// <summary>
    /// Update DTO for Commune
    /// </summary>
    public class CommuneUpdateDto
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(5)]
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

        public CommuneLevel Level { get; set; }

        public string DistrictCode { get; set; } = string.Empty;
    }
}
