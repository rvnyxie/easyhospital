using Refined.EasyHospital.Base;

namespace Refined.EasyHospital.Communes
{
    /// <summary>
    /// Commune entity
    /// </summary>
    public class Commune : BaseLocality
    {
        public CommuneLevel Level { get; set; }

        public string DistrictCode { get; set; } = string.Empty;
    }
}
