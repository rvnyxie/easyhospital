using System;

namespace Refined.EasyHospital.Communes
{
    /// <summary>
    /// Commune DTO
    /// </summary>
    public class CommuneDto
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string? EnglishName { get; set; }

        public DateTime? DecisionDate { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public int Population { get; set; }

        public float Area { get; set; }

        public string? Description { get; set; }

        public CommuneLevel Level { get; set; }

        public string DistrictCode { get; set; } = string.Empty;
    }
}
