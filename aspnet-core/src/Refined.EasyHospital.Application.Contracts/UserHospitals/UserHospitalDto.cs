using System;
using Volo.Abp.Application.Dtos;

namespace Refined.EasyHospital.UserHospitals
{
    /// <summary>
    /// UserHospital DTO
    /// </summary>
    public class UserHospitalDto : AuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }

        public Guid HospitalId { get; set; }
    }
}
