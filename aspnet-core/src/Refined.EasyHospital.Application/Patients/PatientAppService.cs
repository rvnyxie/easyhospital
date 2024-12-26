using Refined.EasyHospital.UserHospitals;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace Refined.EasyHospital.Patients
{
    /// <summary>
    /// App service implementation for patient
    /// </summary>
    /// <param name="patientRepository">Patient EF Core repository</param>
    /// <param name="patientDapperRepository">Patient Dapper repository</param>
    public class PatientAppService(
        IRepository<Patient, Guid> patientRepository,
        IPatientDapperRepository patientDapperRepository,
        IUserHospitalDapperRepository userHospitalDapperRepository,
        ICurrentUser currentUser)
        :
        CrudAppService<Patient, PatientDto, Guid, PatientPagedAndSortedResultRequestDto, PatientCreateDto, PatientUpdateDto>(patientRepository),
        IPatientAppService
    {
        public override async Task<PagedResultDto<PatientDto>> GetListAsync(PatientPagedAndSortedResultRequestDto input)
        {
            var hospitalId = await GetHospitalIdCorrespondingToCurrentLoggedInUserAsync();

            // Extract pagination and filter parameters
            var search = input.Search;
            var provinceCode = input.ProvinceCode;
            var districtCode = input.DistrictCode;
            var communeCode = input.CommuneCode;
            var pageSize = input.MaxResultCount;
            var currentPage = input.SkipCount / input.MaxResultCount + 1;

            var (patients, totalCount) = await patientDapperRepository.GetManyPatientsWithPaginationAndSearch(search, pageSize, currentPage, provinceCode, districtCode, communeCode, hospitalId);

            var patientDtos = await MapToGetListOutputDtosAsync(patients);

            return new PagedResultDto<PatientDto>(
                totalCount,
                patientDtos
            );
        }

        public override async Task<PatientDto> GetAsync(Guid id)
        {
            var hospitalId = await GetHospitalIdCorrespondingToCurrentLoggedInUserAsync();

            var patient = await patientDapperRepository.GetPatientByIdAsync(id, hospitalId);

            var patientDto = await MapToGetListOutputDtoAsync(patient);

            return patientDto;
        }

        public override async Task<PatientDto> CreateAsync(PatientCreateDto patientCreateDto)
        {
            var hospitalId = await GetHospitalIdCorrespondingToCurrentLoggedInUserAsync();

            patientCreateDto.HospitalId = hospitalId;

            var createdPatient = await patientRepository.InsertAsync(MapToEntity(patientCreateDto));

            return MapToGetListOutputDto(createdPatient);
        }

        public override async Task<PatientDto> UpdateAsync(Guid id, PatientUpdateDto patientUpdateDto)
        {
            var hospitalId = await GetHospitalIdCorrespondingToCurrentLoggedInUserAsync();

            patientUpdateDto.HospitalId = hospitalId;

            var currentExistedPatient = await patientRepository.GetAsync(id);

            // TODO: should get only current patient with logged in user's hospital ID
            if (currentExistedPatient == null || currentExistedPatient.HospitalId != hospitalId)
            {
                throw new Exception("Patient not found to update!");
            }

            var patientToUpdate = ObjectMapper.Map<PatientUpdateDto, Patient>(patientUpdateDto, currentExistedPatient);

            var updatedPatient = await patientRepository.UpdateAsync(patientToUpdate);

            return MapToGetListOutputDto(updatedPatient);
        }

        public override async Task DeleteAsync(Guid id)
        {
            var hospitalId = await GetHospitalIdCorrespondingToCurrentLoggedInUserAsync();

            var currentExistedPatient = await patientRepository.GetAsync(id);

            // TODO: should get only current patient with logged in user's hospital ID
            if (currentExistedPatient == null || currentExistedPatient.HospitalId != hospitalId)
            {
                throw new Exception("Patient not found to delete!");
            }

            await patientRepository.DeleteAsync(id);
        }

        private async Task<Guid> GetHospitalIdCorrespondingToCurrentLoggedInUserAsync()
        {
            // Get hospital ID from current logged in user
            var userId = currentUser.Id;

            if (userId == Guid.Empty || userId == null)
            {
                throw new AbpAuthorizationException("You must be logged in to perform this action");
            }

            var currentUserHospital = await userHospitalDapperRepository.GetByUserIdAsync((Guid)userId);

            if (currentUserHospital == null)
            {
                throw new Exception("Current logged in user not belong to any hospital");
            }

            return currentUserHospital.HospitalId;
        }
    }
}
