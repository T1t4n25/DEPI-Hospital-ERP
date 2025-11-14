using AutoMapper;
using HospitalERP.API.Features.Services.Dtos;
using HospitalERP.API.Models.Entities;

namespace HospitalERP.API.Features.Services.Mappings;

public class ServicesProfile : Profile
{
    public ServicesProfile()
    {
        CreateMap<CreateServiceDto, Service>();
        CreateMap<UpdateServiceDto, Service>();
        CreateMap<Service, ServiceListDto>()
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.DepartmentName));
        CreateMap<Service, ServiceDetailDto>()
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.DepartmentName));
    }
}

