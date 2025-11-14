using AutoMapper;
using HospitalERP.API.Common.Exceptions;
using HospitalERP.API.Data;
using HospitalERP.API.Features.Departments.Dtos;
using HospitalERP.API.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalERP.API.Features.Departments.Queries;

public class GetDepartmentByIdQueryHandler : IRequestHandler<GetDepartmentByIdQuery, DepartmentDetailDto>
{
    private readonly HospitalDbContext _context;
    private readonly IMapper _mapper;

    public GetDepartmentByIdQueryHandler(HospitalDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<DepartmentDetailDto> Handle(
        GetDepartmentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var department = await _context.Departments
            .AsNoTracking()
            .Include(d => d.Manager)
            .FirstOrDefaultAsync(d => d.DepartmentID == request.DepartmentID, cancellationToken);

        if (department == null)
        {
            throw new NotFoundException(nameof(Department), request.DepartmentID);
        }

        return _mapper.Map<DepartmentDetailDto>(department);
    }
}

