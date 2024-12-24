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
        CrudAppService<Hospital, HospitalDto, Guid, HospitalPagedAndSortedResultRequestDto, HospitalCreateDto, HospitalUpdateDto>(hospitalRepository),
        IHospitalAppService
    {
        public override async Task<PagedResultDto<HospitalDto>> GetListAsync(HospitalPagedAndSortedResultRequestDto input)
        {
            var search = input.Search;
            var provinceCode = input.ProvinceCode;
            var districtCode = input.DistrictCode;
            var communeCode = input.CommuneCode;
            var pageSize = input.MaxResultCount;
            var currentPage = input.SkipCount / input.MaxResultCount + 1;

            var hospitals = await hospitalDapperRepository.GetManyHospitalWithPaginationAndSearch(search, pageSize, currentPage, provinceCode, districtCode, communeCode);

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
