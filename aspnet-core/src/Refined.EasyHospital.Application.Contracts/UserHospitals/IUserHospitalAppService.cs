using Refined.EasyHospital.Base;
using System;
using Volo.Abp.Application.Services;

namespace Refined.EasyHospital.UserHospitals
{
    /// <summary>
    /// App service interface for user-hospital
    /// </summary>
    public interface IUserHospitalAppService :
        ICrudAppService<UserHospitalDto, Guid, ExtendedPagedAndSortedResultRequestDto, UserHospitalCreateDto, UserHospitalUpdateDto>
    {
    }
}
