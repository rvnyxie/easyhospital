using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Refined.EasyHospital.Communes
{
    /// <summary>
    /// App service implementation for Commune
    /// </summary>
    /// <param name="communeRepository">EF Core commune repository</param>
    /// <param name="communeDapperRepository">Dapper commune repository</param>
    public class CommuneAppService(
        IRepository<Commune, Guid> communeRepository,
        ICommuneDapperRepository communeDapperRepository)
        :
        CrudAppService<Commune, CommuneDto, Guid, PagedAndSortedResultRequestDto, CommuneCreateDto, CommuneUpdateDto>(communeRepository),
        ITransientDependency,
        ICommuneAppService
    {
        public override async Task<PagedResultDto<CommuneDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var communes = await communeDapperRepository.GetManyAsync();

            var communeDtos = await MapToGetListOutputDtosAsync(communes);

            var totalCount = communeDtos.Count;

            return new PagedResultDto<CommuneDto>(
                totalCount,
                communeDtos
            );
        }

        public override async Task<CommuneDto> GetAsync(Guid id)
        {
            var commune = await communeDapperRepository.GetAsync(id);

            var communeDto = await MapToGetOutputDtoAsync(commune);

            return communeDto;
        }
    }
}
