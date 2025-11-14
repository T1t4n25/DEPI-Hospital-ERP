using AutoMapper;
using HospitalERP.API.Features.Employees.Dtos;
using HospitalERP.API.Models.Entities;

namespace HospitalERP.API.Features.Employees.Mappings;

public class EmployeesProfile : Profile
{
    public EmployeesProfile()
    {
        CreateMap<CreateEmployeeDto, Employee>();
        CreateMap<UpdateEmployeeDto, Employee>();
        CreateMap<Employee, EmployeeListDto>()
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.DepartmentName));
        CreateMap<Employee, EmployeeDetailDto>()
            .ForMember(dest => dest.GenderName, opt => opt.MapFrom(src => src.Gender.GenderName))
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.DepartmentName));
    }
}

