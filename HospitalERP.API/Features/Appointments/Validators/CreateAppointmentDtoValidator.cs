using FluentValidation;
using HospitalERP.API.Features.Appointments.Dtos;

namespace HospitalERP.API.Features.Appointments.Validators;

public class CreateAppointmentDtoValidator : AbstractValidator<CreateAppointmentDto>
{
    public CreateAppointmentDtoValidator()
    {
        RuleFor(x => x.PatientID)
            .GreaterThan(0).WithMessage("Patient ID must be greater than 0");

        RuleFor(x => x.DoctorID)
            .GreaterThan(0).WithMessage("Doctor ID must be greater than 0");

        RuleFor(x => x.ServiceID)
            .GreaterThan(0).WithMessage("Service ID must be greater than 0");

        RuleFor(x => x.AppointmentDateTime)
            .NotEmpty().WithMessage("Appointment date and time is required")
            .GreaterThan(DateTime.Now).WithMessage("Appointment date and time must be in the future");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required")
            .MaximumLength(50).WithMessage("Status cannot exceed 50 characters")
            .Must(status => new[] { "Scheduled", "Completed", "Cancelled", "NoShow" }.Contains(status))
            .WithMessage("Status must be one of: Scheduled, Completed, Cancelled, NoShow");
    }
}

public class UpdateAppointmentDtoValidator : AbstractValidator<UpdateAppointmentDto>
{
    public UpdateAppointmentDtoValidator()
    {
        Include(new CreateAppointmentDtoValidator());
        
        RuleFor(x => x.AppointmentID)
            .GreaterThan(0).WithMessage("Appointment ID must be greater than 0");
    }
}

