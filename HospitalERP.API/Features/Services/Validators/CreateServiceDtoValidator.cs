using FluentValidation;
using HospitalERP.API.Features.Services.Dtos;

namespace HospitalERP.API.Features.Services.Validators;

public class CreateServiceDtoValidator : AbstractValidator<CreateServiceDto>
{
    public CreateServiceDtoValidator()
    {
        RuleFor(x => x.ServiceName)
            .NotEmpty().WithMessage("Service name is required")
            .MaximumLength(200).WithMessage("Service name cannot exceed 200 characters");

        RuleFor(x => x.Cost)
            .GreaterThanOrEqualTo(0).WithMessage("Cost must be greater than or equal to 0");

        RuleFor(x => x.DepartmentID)
            .GreaterThan(0).WithMessage("Department ID must be greater than 0");
    }
}

public class UpdateServiceDtoValidator : AbstractValidator<UpdateServiceDto>
{
    public UpdateServiceDtoValidator()
    {
        Include(new CreateServiceDtoValidator());
        
        RuleFor(x => x.ServiceID)
            .GreaterThan(0).WithMessage("Service ID must be greater than 0");
    }
}

