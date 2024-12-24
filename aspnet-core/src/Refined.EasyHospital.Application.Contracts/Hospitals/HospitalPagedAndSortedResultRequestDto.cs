using Refined.EasyHospital.Base;

namespace Refined.EasyHospital.Hospitals
{
    /// <summary>
    /// Paged and sorted result request DTO for hospital
    /// </summary>
    public class HospitalPagedAndSortedResultRequestDto : ExtendedPagedAndSortedResultRequestDto
    {
        public string? ProvinceCode { get; set; }

        public string? DistrictCode { get; set; }

        public string? CommuneCode { get; set; }
    }
}
