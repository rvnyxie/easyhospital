using Refined.EasyHospital.Base;

namespace Refined.EasyHospital.Patients
{
    /// <summary>
    /// Paged and sorted result request DTO for patient
    /// </summary>
    public class PatientPagedAndSortedResultRequestDto : ExtendedPagedAndSortedResultRequestDto
    {
        public string? ProvinceCode { get; set; }

        public string? DistrictCode { get; set; }

        public string? CommuneCode { get; set; }
    }
}
