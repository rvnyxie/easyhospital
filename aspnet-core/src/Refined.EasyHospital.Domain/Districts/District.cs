using Refined.EasyHospital.Base;
using System;

namespace Refined.EasyHospital.Districts
{
    /// <summary>
    /// District entity
    /// </summary>
    public class District : BaseLocality
    {
        public DistrictLevel Level { get; set; }

        public Guid ProvinceId { get; set; }
    }
}
