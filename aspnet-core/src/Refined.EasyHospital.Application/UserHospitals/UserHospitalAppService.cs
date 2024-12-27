using Refined.EasyHospital.Base;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Refined.EasyHospital.UserHospitals
{
    /// <summary>
    /// App service implementation for user-hospital
    /// </summary>
    /// <param name="userHospitalRepository">User-hospital EF Core repository</param>
    /// <param name="userHospitalDapperRepository">User-hospital Dapper repository</param>
    public class UserHospitalAppService(
        IRepository<UserHospital, Guid> userHospitalRepository,
        IUserHospitalDapperRepository userHospitalDapperRepository)
        :
        CrudAppService<UserHospital, UserHospitalDto, Guid, ExtendedPagedAndSortedResultRequestDto, UserHospitalCreateDto, UserHospitalUpdateDto>(userHospitalRepository),
        IUserHospitalAppService
    {
        public override async Task<PagedResultDto<UserHospitalDto>> GetListAsync(ExtendedPagedAndSortedResultRequestDto input)
        {
            // Extract pagination and filter parameters
            var search = input.Search;
            var pageSize = input.MaxResultCount;
            var currentPage = input.SkipCount / input.MaxResultCount + 1;

            var (userHospitalDtos, totalCount) = await userHospitalDapperRepository.GetManyDtosAsync(search, pageSize, currentPage);

            return new PagedResultDto<UserHospitalDto>(
                totalCount,
                userHospitalDtos
            );
        }

        public override async Task<UserHospitalDto> GetAsync(Guid id)
        {
            var userHospital = await userHospitalDapperRepository.GetAsync(id);

            var userHospitalDto = await MapToGetListOutputDtoAsync(userHospital);

            return userHospitalDto;
        }

        public override async Task<UserHospitalDto> CreateAsync(UserHospitalCreateDto userHospitalCreateDto)
        {
            var userHospital = MapToEntity(userHospitalCreateDto);

            // Validate business
            var foundUserHospital = await userHospitalDapperRepository.GetByUserIdAsync(userHospitalCreateDto.UserId);

            if (foundUserHospital != null)
            {
                throw new Exception("Unable to create new user-hospital. User with this ID already belongs to a hospital");
            }

            var createdHospital = await userHospitalRepository.InsertAsync(userHospital);

            return MapToGetListOutputDto(createdHospital);
        }
    }
}
