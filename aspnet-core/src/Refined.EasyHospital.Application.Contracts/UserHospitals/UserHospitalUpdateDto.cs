using System;
using System.ComponentModel.DataAnnotations;

namespace Refined.EasyHospital.UserHospitals
{
    /// <summary>
    /// Update DTO for user-hospital
    /// </summary>
    public class UserHospitalUpdateDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid HospitalId { get; set; }
    }
}
