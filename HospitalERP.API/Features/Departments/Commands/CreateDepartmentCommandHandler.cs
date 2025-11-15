using AutoMapper;
using HospitalERP.API.Common.Exceptions;
using HospitalERP.API.Data;
using HospitalERP.API.Features.Departments.Dtos;
using HospitalERP.API.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalERP.API.Features.Departments.Commands;

public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, DepartmentDetailDto>
{
    private readonly HospitalDbContext _context;
    private readonly IMapper _mapper;

    public CreateDepartmentCommandHandler(HospitalDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<DepartmentDetailDto> Handle(
        CreateDepartmentCommand request,
        CancellationToken cancellationToken)
    {
        if (request.Dto.ManagerID.HasValue)
        {
            var employeeExists = await _context.Employees
                .AnyAsync(e => e.EmployeeID == request.Dto.ManagerID.Value, cancellationToken);
            if (!employeeExists)
            {
                throw new NotFoundException(nameof(Employee), request.Dto.ManagerID.Value);
            }
        }

        var department = _mapper.Map<Department>(request.Dto);
        _context.Departments.Add(department);
        await _context.SaveChangesAsync(cancellationToken);

        var createdDepartment = await _context.Departments
            .Include(d => d.Manager)
            .FirstAsync(d => d.DepartmentID == department.DepartmentID, cancellationToken);

        return _mapper.Map<DepartmentDetailDto>(createdDepartment);
    }
}

