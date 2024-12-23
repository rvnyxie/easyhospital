using Refined.EasyHospital.Base;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Refined.EasyHospital.Provinces
{
    /// <summary>
    /// Service implementation for Province
    /// </summary>
    /// <param name="provinceRepository">Province repository</param>
    public class ProvinceAppService(
        IRepository<Province, Guid> provinceRepository,
        IProvinceDapperRepository provinceDapperRepository)
        :
        CrudAppService<Province, ProvinceDto, Guid, LocalityPagedAndSortedResultRequestDto, ProvinceCreateDto, ProvinceUpdateDto>(provinceRepository),
        IProvinceAppService
    {

        public override async Task<PagedResultDto<ProvinceDto>> GetListAsync(LocalityPagedAndSortedResultRequestDto input)
        {
            // Extract pagination and filter parameters
            var search = input.Search;
            var pageSize = input.MaxResultCount;
            var currentPage = input.SkipCount / input.MaxResultCount + 1;

            var provinces = await provinceDapperRepository.GetManyAsync(search, pageSize, currentPage);

            var totalCount = provinces.Count();

            var provinceDtos = await MapToGetListOutputDtosAsync(provinces);

            return new PagedResultDto<ProvinceDto>(
                totalCount,
                provinceDtos
            );
        }

        public async override Task<ProvinceDto> GetAsync(Guid id)
        {
            var province = await provinceDapperRepository.GetAsync(id);

            var provinceDto = await MapToGetOutputDtoAsync(province);

            return provinceDto;
        }
    }
}
