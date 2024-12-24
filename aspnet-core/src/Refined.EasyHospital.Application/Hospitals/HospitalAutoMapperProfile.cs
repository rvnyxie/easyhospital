using AutoMapper;

namespace Refined.EasyHospital.Hospitals
{
    /// <summary>
    /// AutoMapper profile for hospital
    /// </summary>
    public class HospitalAutoMapperProfile : Profile
    {
        public HospitalAutoMapperProfile()
        {
            // To DTO
            CreateMap<Hospital, HospitalDto>();

            // To entity
            CreateMap<HospitalCreateDto, Hospital>();
            CreateMap<HospitalUpdateDto, Hospital>();
        }
    }
}
