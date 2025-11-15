using AutoMapper;
using HospitalERP.API.Common.Exceptions;
using HospitalERP.API.Data;
using HospitalERP.API.Features.Employees.Dtos;
using HospitalERP.API.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalERP.API.Features.Employees.Queries;

public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeDetailDto>
{
    private readonly HospitalDbContext _context;
    private readonly IMapper _mapper;

    public GetEmployeeByIdQueryHandler(HospitalDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<EmployeeDetailDto> Handle(
        GetEmployeeByIdQuery request,
        CancellationToken cancellationToken)
    {
        var employee = await _context.Employees
            .AsNoTracking()
            .Include(e => e.Gender)
            .Include(e => e.Role)
            .Include(e => e.Department)
            .FirstOrDefaultAsync(e => e.EmployeeID == request.EmployeeID, cancellationToken);

        if (employee == null)
        {
            throw new NotFoundException(nameof(Employee), request.EmployeeID);
        }

        return _mapper.Map<EmployeeDetailDto>(employee);
    }
}

