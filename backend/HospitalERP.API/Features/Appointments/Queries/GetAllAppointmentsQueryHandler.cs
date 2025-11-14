using AutoMapper;
using HospitalERP.API.Common.Pagination;
using HospitalERP.API.Data;
using HospitalERP.API.Features.Appointments.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalERP.API.Features.Appointments.Queries;

public class GetAllAppointmentsQueryHandler : IRequestHandler<GetAllAppointmentsQuery, PaginatedResponse<AppointmentListDto>>
{
    private readonly HospitalDbContext _context;
    private readonly IMapper _mapper;

    public GetAllAppointmentsQueryHandler(HospitalDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedResponse<AppointmentListDto>> Handle(
        GetAllAppointmentsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Appointments
            .AsNoTracking()
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .Include(a => a.Service)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.QueryParams.SearchTerm))
        {
            var searchTerm = request.QueryParams.SearchTerm.ToLower();
            query = query.Where(a =>
                a.Patient.FirstName.ToLower().Contains(searchTerm) ||
                a.Patient.LastName.ToLower().Contains(searchTerm) ||
                a.Doctor.FirstName.ToLower().Contains(searchTerm) ||
                a.Doctor.LastName.ToLower().Contains(searchTerm));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var appointments = await query
            .OrderByDescending(a => a.AppointmentDateTime)
            .Skip((request.QueryParams.PageNumber - 1) * request.QueryParams.PageSize)
            .Take(request.QueryParams.PageSize)
            .ToListAsync(cancellationToken);

        var appointmentDtos = _mapper.Map<List<AppointmentListDto>>(appointments);

        return new PaginatedResponse<AppointmentListDto>(
            appointmentDtos,
            request.QueryParams.PageNumber,
            request.QueryParams.PageSize,
            totalCount);
    }
}

