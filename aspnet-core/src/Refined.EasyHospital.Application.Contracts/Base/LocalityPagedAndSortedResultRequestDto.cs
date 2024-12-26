namespace Refined.EasyHospital.Base
{
    /// <summary>
    /// Paged result request DTO for Locality entities
    /// </summary>
    public class LocalityPagedAndSortedResultRequestDto : ExtendedPagedAndSortedResultRequestDto
    {
        public string? ProvinceCode { get; set; }

        public string? DistrictCode { get; set; }

        public string? CommuneCode { get; set; }
    }
}
