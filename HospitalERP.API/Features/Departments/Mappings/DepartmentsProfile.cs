using AutoMapper;
using HospitalERP.API.Features.Departments.Dtos;
using HospitalERP.API.Models.Entities;

namespace HospitalERP.API.Features.Departments.Mappings;

public class DepartmentsProfile : Profile
{
    public DepartmentsProfile()
    {
        CreateMap<CreateDepartmentDto, Department>();
        CreateMap<UpdateDepartmentDto, Department>();
        CreateMap<Department, DepartmentListDto>()
            .ForMember(dest => dest.ManagerName, opt => opt.MapFrom(src => 
                src.Manager != null ? $"{src.Manager.FirstName} {src.Manager.LastName}" : null));
        CreateMap<Department, DepartmentDetailDto>()
            .ForMember(dest => dest.ManagerName, opt => opt.MapFrom(src => 
                src.Manager != null ? $"{src.Manager.FirstName} {src.Manager.LastName}" : null));
    }
}

