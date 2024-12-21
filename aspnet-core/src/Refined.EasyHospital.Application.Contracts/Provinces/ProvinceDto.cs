using System;
using Volo.Abp.Application.Dtos;

namespace Refined.EasyHospital.Provinces
{
    /// <summary>
    /// Province DTO
    /// </summary>
    public class ProvinceDto : AuditedEntityDto<Guid>
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string? EnglishName { get; set; }

        public DateOnly? DecisionDate { get; set; }

        public DateOnly? EffectiveDate { get; set; }

        public int Population { get; set; }

        public float Area { get; set; }

        public string? Description { get; set; }

        public ProvinceLevel Level { get; set; }
    }
}
