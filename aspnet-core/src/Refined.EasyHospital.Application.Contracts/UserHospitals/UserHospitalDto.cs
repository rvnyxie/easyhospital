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

        public string UserName { get; set; } = string.Empty;

        public Guid HospitalId { get; set; }

        public string HospitalName { get; set; } = string.Empty;
    }
}
