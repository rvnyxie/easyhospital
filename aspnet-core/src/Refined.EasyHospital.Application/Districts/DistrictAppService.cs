using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Refined.EasyHospital.Districts
{
    /// <summary>
    /// App service implementation for District
    /// </summary>
    public class DistrictAppService(
        IRepository<District, Guid> districtRepository,
        IDistrictDapperRepository districtDapperRepository)
        :
        CrudAppService<District, DistrictDto, Guid, PagedAndSortedResultRequestDto, DistrictCreateDto, DistrictUpdateDto>(districtRepository),
        IDistrictAppService
    {
        public override async Task<PagedResultDto<DistrictDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var districts = await districtDapperRepository.GetManyAsync();

            var districtDtos = await MapToGetListOutputDtosAsync(districts);

            var totalCount = districtDtos.Count;

            return new PagedResultDto<DistrictDto>(
                totalCount,
                districtDtos
            );
        }

        public override async Task<DistrictDto> GetAsync(Guid id)
        {
            var district = await districtDapperRepository.GetAsync(id);

            var districtDto = await MapToGetOutputDtoAsync(district);

            return districtDto;
        }
    }
}
