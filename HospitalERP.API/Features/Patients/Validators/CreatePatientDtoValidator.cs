using FluentValidation;
using HospitalERP.API.Features.Patients.Dtos;

namespace HospitalERP.API.Features.Patients.Validators;

public class CreatePatientDtoValidator : AbstractValidator<CreatePatientDto>
{
    public CreatePatientDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(100).WithMessage("First name cannot exceed 100 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required")
            .LessThan(DateOnly.FromDateTime(DateTime.Today)).WithMessage("Date of birth must be in the past")
            .Must(dob => DateTime.Today.Year - dob.Year <= 150).WithMessage("Date of birth is invalid");

        RuleFor(x => x.GenderID)
            .GreaterThan(0).WithMessage("Gender ID must be greater than 0");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required")
            .MaximumLength(500).WithMessage("Address cannot exceed 500 characters");

        RuleFor(x => x.BloodTypeID)
            .GreaterThan(0).WithMessage("Blood type ID must be greater than 0");

        RuleFor(x => x.ContactNumber)
            .NotEmpty().WithMessage("Contact number is required")
            .MaximumLength(20).WithMessage("Contact number cannot exceed 20 characters");
    }
}

public class UpdatePatientDtoValidator : AbstractValidator<UpdatePatientDto>
{
    public UpdatePatientDtoValidator()
    {
        Include(new CreatePatientDtoValidator());
        
        RuleFor(x => x.PatientID)
            .GreaterThan(0).WithMessage("Patient ID must be greater than 0");
    }
}

