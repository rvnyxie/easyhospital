using System;
using System.ComponentModel.DataAnnotations;

namespace Refined.EasyHospital.UserHospitals
{
    /// <summary>
    /// Creation DTO for user-hospital
    /// </summary>
    public class UserHospitalCreateDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid HospitalId { get; set; }
    }
}
