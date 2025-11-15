using FluentValidation;
using HospitalERP.API.Features.Invoices.Dtos;

namespace HospitalERP.API.Features.Invoices.Validators;

public class CreateHospitalInvoiceItemDtoValidator : AbstractValidator<CreateHospitalInvoiceItemDto>
{
    public CreateHospitalInvoiceItemDtoValidator()
    {
        RuleFor(x => x.ServiceID)
            .GreaterThan(0).WithMessage("Service ID must be greater than 0");

        RuleFor(x => x.LineTotal)
            .GreaterThanOrEqualTo(0).WithMessage("Line total must be greater than or equal to 0");
    }
}

public class CreateMedicationInvoiceItemDtoValidator : AbstractValidator<CreateMedicationInvoiceItemDto>
{
    public CreateMedicationInvoiceItemDtoValidator()
    {
        RuleFor(x => x.MedicationID)
            .GreaterThan(0).WithMessage("Medication ID must be greater than 0");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0");

        RuleFor(x => x.LineTotal)
            .GreaterThanOrEqualTo(0).WithMessage("Line total must be greater than or equal to 0");
    }
}

public class CreateInvoiceDtoValidator : AbstractValidator<CreateInvoiceDto>
{
    public CreateInvoiceDtoValidator()
    {
        RuleFor(x => x.PatientID)
            .GreaterThan(0).WithMessage("Patient ID must be greater than 0");

        RuleFor(x => x.InvoiceTypeID)
            .GreaterThan(0).WithMessage("Invoice type ID must be greater than 0");

        RuleFor(x => x.InvoiceDate)
            .NotEmpty().WithMessage("Invoice date is required")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today)).WithMessage("Invoice date cannot be in the future");

        RuleFor(x => x.PaymentStatusID)
            .GreaterThan(0).WithMessage("Payment status ID must be greater than 0");

        RuleFor(x => x.HospitalItems)
            .NotNull().WithMessage("Hospital items cannot be null")
            .Must(items => items.Count >= 0).WithMessage("Hospital items list is invalid");

        RuleForEach(x => x.HospitalItems)
            .SetValidator(new CreateHospitalInvoiceItemDtoValidator());

        RuleFor(x => x.MedicationItems)
            .NotNull().WithMessage("Medication items cannot be null")
            .Must(items => items.Count >= 0).WithMessage("Medication items list is invalid");

        RuleForEach(x => x.MedicationItems)
            .SetValidator(new CreateMedicationInvoiceItemDtoValidator());

        RuleFor(x => x)
            .Must(x => x.HospitalItems.Count > 0 || x.MedicationItems.Count > 0)
            .WithMessage("Invoice must have at least one hospital item or medication item");
    }
}

public class UpdateInvoiceDtoValidator : AbstractValidator<UpdateInvoiceDto>
{
    public UpdateInvoiceDtoValidator()
    {
        RuleFor(x => x.InvoiceID)
            .GreaterThan(0).WithMessage("Invoice ID must be greater than 0");

        RuleFor(x => x.PaymentStatusID)
            .GreaterThan(0).WithMessage("Payment status ID must be greater than 0");

        RuleFor(x => x.PayDate)
            .GreaterThan(DateOnly.FromDateTime(DateTime.MinValue))
            .When(x => x.PayDate.HasValue)
            .WithMessage("Pay date is invalid");
    }
}

