using AutoMapper;
using HospitalERP.API.Common.Exceptions;
using HospitalERP.API.Data;
using HospitalERP.API.Features.Services.Dtos;
using HospitalERP.API.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalERP.API.Features.Services.Commands;

public class UpdateServiceCommandHandler : IRequestHandler<UpdateServiceCommand, ServiceDetailDto>
{
    private readonly HospitalDbContext _context;
    private readonly IMapper _mapper;

    public UpdateServiceCommandHandler(HospitalDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ServiceDetailDto> Handle(
        UpdateServiceCommand request,
        CancellationToken cancellationToken)
    {
        var service = await _context.Services
            .FirstOrDefaultAsync(s => s.ServiceID == request.Dto.ServiceID, cancellationToken);

        if (service == null)
        {
            throw new NotFoundException(nameof(Service), request.Dto.ServiceID);
        }

        var departmentExists = await _context.Departments
            .AnyAsync(d => d.DepartmentID == request.Dto.DepartmentID, cancellationToken);
        if (!departmentExists)
        {
            throw new NotFoundException(nameof(Department), request.Dto.DepartmentID);
        }

        _mapper.Map(request.Dto, service);
        await _context.SaveChangesAsync(cancellationToken);

        var updatedService = await _context.Services
            .Include(s => s.Department)
            .FirstAsync(s => s.ServiceID == service.ServiceID, cancellationToken);

        return _mapper.Map<ServiceDetailDto>(updatedService);
    }
}

