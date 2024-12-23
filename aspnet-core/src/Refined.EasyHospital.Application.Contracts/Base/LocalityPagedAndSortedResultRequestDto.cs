using Volo.Abp.Application.Dtos;

namespace Refined.EasyHospital.Base
{
    /// <summary>
    /// Paged result request DTO for Locality entities
    /// </summary>
    public class LocalityPagedAndSortedResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string? Search { get; set; }
    }
}
