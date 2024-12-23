using Refined.EasyHospital.Provinces;

namespace Refined.EasyHospital.Helpers
{
    /// <summary>
    /// Mapping helper for enum
    /// </summary>
    public class EnumMappingHelper
    {
        public bool TryMapProvinceLevel(string levelText, out ProvinceLevel level)
        {
            level = default;

            switch (levelText.Trim())
            {
                case "Tỉnh":
                    level = ProvinceLevel.Province;
                    return true;
                case "Thành phố Trung ương":
                    level = ProvinceLevel.MunipicalCity;
                    return true;
                default:
                    return false;
            }
        }
    }
}
