using Refined.EasyHospital.Base;
using System;

namespace Refined.EasyHospital.Communes
{
    /// <summary>
    /// Commune entity
    /// </summary>
    public class Commune : BaseLocality
    {
        public CommuneLevel Level { get; set; }

        public Guid DistrictId { get; set; }
    }
}
