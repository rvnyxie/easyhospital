using AutoMapper;

namespace Refined.EasyHospital.Districts
{
    /// <summary>
    /// AutoMapper profile for District
    /// </summary>
    public class DistrictAutoMapperProfile : Profile
    {
        public DistrictAutoMapperProfile()
        {
            // To DTO
            CreateMap<District, DistrictDto>();

            // To entity
            CreateMap<DistrictCreateDto, District>();
            CreateMap<DistrictUpdateDto, District>();
        }
    }
}
