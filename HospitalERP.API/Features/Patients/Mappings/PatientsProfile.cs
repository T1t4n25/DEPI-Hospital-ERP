using AutoMapper;
using HospitalERP.API.Features.Patients.Dtos;
using HospitalERP.API.Models.Entities;

namespace HospitalERP.API.Features.Patients.Mappings;

public class PatientsProfile : Profile
{
    public PatientsProfile()
    {
        CreateMap<CreatePatientDto, Patient>();
        CreateMap<UpdatePatientDto, Patient>();
        CreateMap<Patient, PatientListDto>()
            .ForMember(dest => dest.GenderName, opt => opt.MapFrom(src => src.Gender.GenderName))
            .ForMember(dest => dest.BloodTypeName, opt => opt.MapFrom(src => src.BloodType.BloodTypeName));
        CreateMap<Patient, PatientDetailDto>()
            .ForMember(dest => dest.GenderName, opt => opt.MapFrom(src => src.Gender.GenderName))
            .ForMember(dest => dest.BloodTypeName, opt => opt.MapFrom(src => src.BloodType.BloodTypeName));
    }
}

