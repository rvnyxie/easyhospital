using AutoMapper;

namespace Refined.EasyHospital.Provinces
{
    /// <summary>
    /// AutoMapper profile for Province
    /// </summary>
    public class ProvinceAutoMapperProfile : Profile
    {
        public ProvinceAutoMapperProfile()
        {
            // To DTO
            CreateMap<Province, ProvinceDto>();

            // To entity
            CreateMap<ProvinceCreateDto, Province>();
            CreateMap<ProvinceUpdateDto, Province>();
        }
    }
}
