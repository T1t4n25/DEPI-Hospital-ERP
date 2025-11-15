using AutoMapper;
using HospitalERP.API.Features.MedicalRecords.Dtos;
using HospitalERP.API.Models.Entities;

namespace HospitalERP.API.Features.MedicalRecords.Mappings;

public class MedicalRecordsProfile : Profile
{
    public MedicalRecordsProfile()
    {
        CreateMap<CreateMedicalRecordDto, MedicalRecord>();
        CreateMap<UpdateMedicalRecordDto, MedicalRecord>();
        CreateMap<MedicalRecord, MedicalRecordListDto>()
            .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => $"{src.Patient.FirstName} {src.Patient.LastName}"))
            .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => $"{src.Doctor.FirstName} {src.Doctor.LastName}"))
            .ForMember(dest => dest.Diagnosis, opt => opt.MapFrom(src => src.Diagnosis.Diagnoses));
        CreateMap<MedicalRecord, MedicalRecordDetailDto>()
            .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => $"{src.Patient.FirstName} {src.Patient.LastName}"))
            .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => $"{src.Doctor.FirstName} {src.Doctor.LastName}"))
            .ForMember(dest => dest.Diagnosis, opt => opt.MapFrom(src => src.Diagnosis.Diagnoses));
    }
}

