using Refined.EasyHospital.Base;

namespace Refined.EasyHospital.Provinces
{
    /// <summary>
    /// Province Entity
    /// </summary>
    public class Province : BaseLocality
    {
        public ProvinceLevel Level { get; set; }
    }
}
