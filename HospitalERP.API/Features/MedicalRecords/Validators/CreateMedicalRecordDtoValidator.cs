using FluentValidation;
using HospitalERP.API.Features.MedicalRecords.Dtos;

namespace HospitalERP.API.Features.MedicalRecords.Validators;

public class CreateMedicalRecordDtoValidator : AbstractValidator<CreateMedicalRecordDto>
{
    public CreateMedicalRecordDtoValidator()
    {
        RuleFor(x => x.PatientID)
            .GreaterThan(0).WithMessage("Patient ID must be greater than 0");

        RuleFor(x => x.DoctorID)
            .GreaterThan(0).WithMessage("Doctor ID must be greater than 0");

        RuleFor(x => x.Diagnosesid)
            .GreaterThan(0).WithMessage("Diagnosis ID must be greater than 0");

        RuleFor(x => x.DiagnoseDate)
            .NotEmpty().WithMessage("Diagnose date is required")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today)).WithMessage("Diagnose date cannot be in the future");
    }
}

public class UpdateMedicalRecordDtoValidator : AbstractValidator<UpdateMedicalRecordDto>
{
    public UpdateMedicalRecordDtoValidator()
    {
        Include(new CreateMedicalRecordDtoValidator());
        
        RuleFor(x => x.RecordID)
            .GreaterThan(0).WithMessage("Record ID must be greater than 0");
    }
}

