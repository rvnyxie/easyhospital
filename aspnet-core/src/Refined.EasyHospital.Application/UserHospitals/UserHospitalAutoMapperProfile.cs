using AutoMapper;

namespace Refined.EasyHospital.UserHospitals
{
    /// <summary>
    /// AutoMapper profile for user-hospital
    /// </summary>
    public class UserHospitalAutoMapperProfile : Profile
    {
        public UserHospitalAutoMapperProfile()
        {
            // To DTO
            CreateMap<UserHospital, UserHospitalDto>();

            // To entity
            CreateMap<UserHospitalCreateDto, UserHospital>();
            CreateMap<UserHospitalUpdateDto, UserHospital>();
        }
    }
}
