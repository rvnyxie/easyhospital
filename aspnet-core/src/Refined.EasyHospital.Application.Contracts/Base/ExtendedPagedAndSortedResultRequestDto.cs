using Volo.Abp.Application.Dtos;

namespace Refined.EasyHospital.Base
{
    /// <summary>
    /// Extended paged and sorted result request dto
    /// </summary>
    public class ExtendedPagedAndSortedResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string? Search { get; set; }
    }
}
