using FluentValidation;
using HospitalERP.API.Features.Departments.Dtos;

namespace HospitalERP.API.Features.Departments.Validators;

public class CreateDepartmentDtoValidator : AbstractValidator<CreateDepartmentDto>
{
    public CreateDepartmentDtoValidator()
    {
        RuleFor(x => x.DepartmentName)
            .NotEmpty().WithMessage("Department name is required")
            .MaximumLength(100).WithMessage("Department name cannot exceed 100 characters");

        RuleFor(x => x.ManagerID)
            .GreaterThan(0).When(x => x.ManagerID.HasValue)
            .WithMessage("Manager ID must be greater than 0 when provided");
    }
}

public class UpdateDepartmentDtoValidator : AbstractValidator<UpdateDepartmentDto>
{
    public UpdateDepartmentDtoValidator()
    {
        Include(new CreateDepartmentDtoValidator());
        
        RuleFor(x => x.DepartmentID)
            .GreaterThan(0).WithMessage("Department ID must be greater than 0");
    }
}

