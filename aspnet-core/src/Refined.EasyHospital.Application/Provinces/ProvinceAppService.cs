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
        CrudAppService<Province, ProvinceDto, Guid, PagedAndSortedResultRequestDto, ProvinceCreateDto, ProvinceUpdateDto>(provinceRepository),
        IProvinceAppService
    {
        public override async Task<PagedResultDto<ProvinceDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var provinces = await provinceDapperRepository.GetManyAsync();

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
