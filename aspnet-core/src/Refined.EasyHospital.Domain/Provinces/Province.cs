using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Refined.EasyHospital.Provinces
{
    /// <summary>
    /// Province Entity
    /// </summary>
    public class Province : AuditedAggregateRoot<Guid>
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
