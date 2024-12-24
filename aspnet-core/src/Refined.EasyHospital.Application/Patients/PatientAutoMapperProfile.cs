using AutoMapper;

namespace Refined.EasyHospital.Patients
{
    /// <summary>
    /// AutoMapper profile for patient
    /// </summary>
    public class PatientAutoMapperProfile : Profile
    {
        public PatientAutoMapperProfile()
        {
            // To DTO
            CreateMap<Patient, PatientDto>();

            // To entity
            CreateMap<PatientCreateDto, Patient>();
            CreateMap<PatientUpdateDto, Patient>();
        }
    }
}
