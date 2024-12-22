using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Refined.EasyHospital.Base
{
    /// <summary>
    /// Base Locality entity providing essential properties for jurisdiction
    /// </summary>
    public class BaseLocality : AuditedAggregateRoot<Guid>
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string? EnglishName { get; set; }

        public DateTime? DecisionDate { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public int Population { get; set; }

        public float Area { get; set; }

        public string? Description { get; set; }
    }
}
