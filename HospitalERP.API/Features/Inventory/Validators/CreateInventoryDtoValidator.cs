using FluentValidation;
using HospitalERP.API.Features.Inventory.Dtos;

namespace HospitalERP.API.Features.Inventory.Validators;

public class CreateInventoryDtoValidator : AbstractValidator<CreateInventoryDto>
{
    public CreateInventoryDtoValidator()
    {
        RuleFor(x => x.MedicationID)
            .GreaterThan(0).WithMessage("Medication ID must be greater than 0");

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0).WithMessage("Quantity must be greater than or equal to 0");

        RuleFor(x => x.ExpiryDate)
            .NotEmpty().WithMessage("Expiry date is required")
            .GreaterThan(DateOnly.FromDateTime(DateTime.Today)).WithMessage("Expiry date must be in the future");
    }
}

public class UpdateInventoryDtoValidator : AbstractValidator<UpdateInventoryDto>
{
    public UpdateInventoryDtoValidator()
    {
        Include(new CreateInventoryDtoValidator());
        // UpdateInventoryDto uses MedicationID as identifier, which is already validated in CreateInventoryDtoValidator
    }
}

