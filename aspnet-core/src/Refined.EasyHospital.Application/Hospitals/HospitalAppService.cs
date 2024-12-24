using Refined.EasyHospital.Base;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Refined.EasyHospital.Hospitals
{
    /// <summary>
    /// App service implementation for hospital
    /// </summary>
    /// <param name="hospitalRepository">Hospital EF Core repository</param>
    /// <param name="hospitalDapperRepository">Hospital Dapper repository</param>
    public class HospitalAppService(
        IRepository<Hospital, Guid> hospitalRepository,
        IHospitalDapperRepository hospitalDapperRepository)
        :
        CrudAppService<Hospital, HospitalDto, Guid, ExtendedPagedAndSortedResultRequestDto, HospitalCreateDto, HospitalUpdateDto>(hospitalRepository),
        IHospitalAppService
    {
        public override async Task<PagedResultDto<HospitalDto>> GetListAsync(ExtendedPagedAndSortedResultRequestDto input)
        {
            var search = input.Search;
            var pageSize = input.MaxResultCount;
            var currentPage = input.SkipCount / input.MaxResultCount + 1;

            var hospitals = await hospitalDapperRepository.GetManyAsync(search, pageSize, currentPage);

            var hospitalDtos = await MapToGetListOutputDtosAsync(hospitals);

            var totalCount = hospitalDtos.Count;

            return new PagedResultDto<HospitalDto>(
                totalCount,
                hospitalDtos
            );
        }

        public override async Task<HospitalDto> GetAsync(Guid id)
        {
            var hospital = await hospitalDapperRepository.GetAsync(id);

            var hospitalDto = await MapToGetOutputDtoAsync(hospital);

            return hospitalDto;
        }
    }
}
