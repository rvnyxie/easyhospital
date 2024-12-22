using AutoMapper;

namespace Refined.EasyHospital.Communes
{
    /// <summary>
    /// AutoMapper Profile for Commune
    /// </summary>
    public class CommuneAutoMapperProfile : Profile
    {
        public CommuneAutoMapperProfile()
        {
            // To DTO
            CreateMap<Commune, CommuneDto>();

            // To entity
            CreateMap<CommuneCreateDto, Commune>();
            CreateMap<CommuneUpdateDto, Commune>();
        }
    }
}
