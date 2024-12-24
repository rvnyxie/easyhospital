using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Refined.EasyHospital.UserHospitals
{
    /// <summary>
    /// UserHospital entity
    /// </summary>
    public class UserHospital : AuditedAggregateRoot<Guid>
    {
        public Guid UserId { get; set; }

        public Guid HospitalId { get; set; }
    }
}
