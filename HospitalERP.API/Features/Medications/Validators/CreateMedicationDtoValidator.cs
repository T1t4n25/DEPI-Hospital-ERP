using FluentValidation;
using HospitalERP.API.Features.Medications.Dtos;

namespace HospitalERP.API.Features.Medications.Validators;

public class CreateMedicationDtoValidator : AbstractValidator<CreateMedicationDto>
{
    public CreateMedicationDtoValidator()
    {
        RuleFor(x => x.BarCode)
            .NotEmpty().WithMessage("Barcode is required")
            .MaximumLength(100).WithMessage("Barcode cannot exceed 100 characters");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Medication name is required")
            .MaximumLength(200).WithMessage("Medication name cannot exceed 200 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");

        RuleFor(x => x.Cost)
            .GreaterThanOrEqualTo(0).WithMessage("Cost must be greater than or equal to 0");
    }
}

public class UpdateMedicationDtoValidator : AbstractValidator<UpdateMedicationDto>
{
    public UpdateMedicationDtoValidator()
    {
        Include(new CreateMedicationDtoValidator());
        
        RuleFor(x => x.MedicationID)
            .GreaterThan(0).WithMessage("Medication ID must be greater than 0");
    }
}

