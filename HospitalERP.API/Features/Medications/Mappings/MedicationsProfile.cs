using AutoMapper;
using HospitalERP.API.Features.Medications.Dtos;
using HospitalERP.API.Models.Entities;

namespace HospitalERP.API.Features.Medications.Mappings;

public class MedicationsProfile : Profile
{
    public MedicationsProfile()
    {
        CreateMap<CreateMedicationDto, Medication>();
        CreateMap<UpdateMedicationDto, Medication>();
        CreateMap<Medication, MedicationListDto>()
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Inventory != null ? src.Inventory.Quantity : (int?)null));
        CreateMap<Medication, MedicationDetailDto>()
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Inventory != null ? src.Inventory.Quantity : (int?)null))
            .ForMember(dest => dest.ExpiryDate, opt => opt.MapFrom(src => src.Inventory != null ? src.Inventory.ExpiryDate : (DateOnly?)null));
    }
}

