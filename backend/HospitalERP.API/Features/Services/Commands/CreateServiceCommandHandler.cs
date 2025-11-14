using AutoMapper;
using HospitalERP.API.Common.Exceptions;
using HospitalERP.API.Data;
using HospitalERP.API.Features.Services.Dtos;
using HospitalERP.API.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalERP.API.Features.Services.Commands;

public class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand, ServiceDetailDto>
{
    private readonly HospitalDbContext _context;
    private readonly IMapper _mapper;

    public CreateServiceCommandHandler(HospitalDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ServiceDetailDto> Handle(
        CreateServiceCommand request,
        CancellationToken cancellationToken)
    {
        var departmentExists = await _context.Departments
            .AnyAsync(d => d.DepartmentID == request.Dto.DepartmentID, cancellationToken);
        if (!departmentExists)
        {
            throw new NotFoundException(nameof(Department), request.Dto.DepartmentID);
        }

        var service = _mapper.Map<Service>(request.Dto);
        _context.Services.Add(service);
        await _context.SaveChangesAsync(cancellationToken);

        var createdService = await _context.Services
            .Include(s => s.Department)
            .FirstAsync(s => s.ServiceID == service.ServiceID, cancellationToken);

        return _mapper.Map<ServiceDetailDto>(createdService);
    }
}

