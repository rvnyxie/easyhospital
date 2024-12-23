using Refined.EasyHospital.Base;

namespace Refined.EasyHospital.Districts
{
    /// <summary>
    /// District entity
    /// </summary>
    public class District : BaseLocality
    {
        public DistrictLevel Level { get; set; }

        public string ProvinceCode { get; set; } = string.Empty;
    }
}
