using AutoMapper;
using HospitalERP.API.Common.Exceptions;
using HospitalERP.API.Data;
using HospitalERP.API.Features.Services.Dtos;
using HospitalERP.API.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalERP.API.Features.Services.Queries;

public class GetServiceByIdQueryHandler : IRequestHandler<GetServiceByIdQuery, ServiceDetailDto>
{
    private readonly HospitalDbContext _context;
    private readonly IMapper _mapper;

    public GetServiceByIdQueryHandler(HospitalDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ServiceDetailDto> Handle(
        GetServiceByIdQuery request,
        CancellationToken cancellationToken)
    {
        var service = await _context.Services
            .AsNoTracking()
            .Include(s => s.Department)
            .FirstOrDefaultAsync(s => s.ServiceID == request.ServiceID, cancellationToken);

        if (service == null)
        {
            throw new NotFoundException(nameof(Service), request.ServiceID);
        }

        return _mapper.Map<ServiceDetailDto>(service);
    }
}

