using Refined.EasyHospital.Communes;
using Refined.EasyHospital.Districts;
using Refined.EasyHospital.Provinces;

namespace Refined.EasyHospital.Helpers
{
    /// <summary>
    /// Mapping helper for enum
    /// </summary>
    public class EnumMappingHelper
    {
        /// <summary>
        /// Try to map from province level text to province level enum
        /// </summary>
        /// <param name="levelText">Province level text</param>
        /// <param name="level">Province level enum</param>
        /// <returns>True if successful to map concrete value, false if not</returns>
        public bool TryMapProvinceLevel(string levelText, out ProvinceLevel level)
        {
            level = default;

            switch (levelText.Trim())
            {
                case "Tỉnh":
                    level = ProvinceLevel.Province;
                    return true;
                case "Thành phố Trung ương":
                    level = ProvinceLevel.MunicipalCity;
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Try to map from district level text to district level enum
        /// </summary>
        /// <param name="levelText">District level text</param>
        /// <param name="level">District level enum</param>
        /// <returns>True if successful to map concrete value, false if not</returns>
        public bool TryMapDistrictLevel(string levelText, out DistrictLevel level)
        {
            level = default;

            switch (levelText.Trim())
            {
                case "Quận":
                    level = DistrictLevel.District;
                    return true;
                case "Huyện":
                    level = DistrictLevel.County;
                    return true;
                case "Thành phố":
                    level = DistrictLevel.City;
                    return true;
                case "Thị xã":
                    level = DistrictLevel.Town;
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Try to map from commune level text to commune level enum
        /// </summary>
        /// <param name="levelText">Commune level text</param>
        /// <param name="level">Commune level enum</param>
        /// <returns>True if successful to map concrete value, false if not</returns>
        public bool TryMapCommuneLevel(string levelText, out CommuneLevel level)
        {
            level = default;

            switch (levelText.Trim())
            {
                case "Xã":
                    level = CommuneLevel.Commune;
                    return true;
                case "Phường":
                    level = CommuneLevel.Ward;
                    return true;
                case "Thị trấn":
                    level = CommuneLevel.Town;
                    return true;
                default:
                    return false;
            }
        }
    }
}
