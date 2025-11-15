using AutoMapper;
using HospitalERP.API.Common.Exceptions;
using HospitalERP.API.Data;
using HospitalERP.API.Features.Appointments.Dtos;
using HospitalERP.API.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalERP.API.Features.Appointments.Commands;

public class UpdateAppointmentCommandHandler : IRequestHandler<UpdateAppointmentCommand, AppointmentDetailDto>
{
    private readonly HospitalDbContext _context;
    private readonly IMapper _mapper;

    public UpdateAppointmentCommandHandler(HospitalDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AppointmentDetailDto> Handle(
        UpdateAppointmentCommand request,
        CancellationToken cancellationToken)
    {
        var appointment = await _context.Appointments
            .FirstOrDefaultAsync(a => a.AppointmentID == request.Dto.AppointmentID, cancellationToken);

        if (appointment == null)
        {
            throw new NotFoundException(nameof(Appointment), request.Dto.AppointmentID);
        }

        // Validate foreign keys
        var patientExists = await _context.Patients
            .AnyAsync(p => p.PatientID == request.Dto.PatientID, cancellationToken);
        if (!patientExists)
        {
            throw new NotFoundException(nameof(Patient), request.Dto.PatientID);
        }

        var doctorExists = await _context.Employees
            .AnyAsync(e => e.EmployeeID == request.Dto.DoctorID, cancellationToken);
        if (!doctorExists)
        {
            throw new NotFoundException(nameof(Employee), request.Dto.DoctorID);
        }

        var serviceExists = await _context.Services
            .AnyAsync(s => s.ServiceID == request.Dto.ServiceID, cancellationToken);
        if (!serviceExists)
        {
            throw new NotFoundException(nameof(Service), request.Dto.ServiceID);
        }

        _mapper.Map(request.Dto, appointment);
        await _context.SaveChangesAsync(cancellationToken);

        var updatedAppointment = await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .Include(a => a.Service)
            .FirstAsync(a => a.AppointmentID == appointment.AppointmentID, cancellationToken);

        return _mapper.Map<AppointmentDetailDto>(updatedAppointment);
    }
}

