using AutoMapper;
using HospitalERP.API.Features.Inventory.Dtos;
using InventoryEntity = HospitalERP.API.Models.Entities.Inventory;

namespace HospitalERP.API.Features.Inventory.Mappings;

public class InventoryProfile : Profile
{
    public InventoryProfile()
    {
        CreateMap<CreateInventoryDto, InventoryEntity>();
        CreateMap<UpdateInventoryDto, InventoryEntity>();
        CreateMap<InventoryEntity, InventoryListDto>()
            .ForMember(dest => dest.MedicationName, opt => opt.MapFrom(src => src.Medication.Name))
            .ForMember(dest => dest.BarCode, opt => opt.MapFrom(src => src.Medication.BarCode));
        CreateMap<InventoryEntity, InventoryDetailDto>()
            .ForMember(dest => dest.MedicationName, opt => opt.MapFrom(src => src.Medication.Name))
            .ForMember(dest => dest.BarCode, opt => opt.MapFrom(src => src.Medication.BarCode))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Medication.Description))
            .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.Medication.Cost));
    }
}

