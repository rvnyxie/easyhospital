using System;
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
    public class ProvinceService(IRepository<Province, Guid> provinceRepository) :
        CrudAppService<Province, ProvinceDto, Guid, PagedAndSortedResultRequestDto, ProvinceCreateDto, ProvinceUpdateDto>(provinceRepository),
        IProvinceService
    {
        public override Task<ProvinceDto> GetAsync(Guid id)
        {
            var result = new ProvinceDto();

            return Task.FromResult(result);
        }
    }
}
