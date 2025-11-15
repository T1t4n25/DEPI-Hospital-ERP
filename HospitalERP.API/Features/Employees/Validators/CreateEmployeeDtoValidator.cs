using FluentValidation;
using HospitalERP.API.Features.Employees.Dtos;

namespace HospitalERP.API.Features.Employees.Validators;

public class CreateEmployeeDtoValidator : AbstractValidator<CreateEmployeeDto>
{
    public CreateEmployeeDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(100).WithMessage("First name cannot exceed 100 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters");

        RuleFor(x => x.GenderID)
            .GreaterThan(0).WithMessage("Gender ID must be greater than 0");

        RuleFor(x => x.RoleID)
            .GreaterThan(0).WithMessage("Role ID must be greater than 0");

        RuleFor(x => x.DepartmentID)
            .GreaterThan(0).WithMessage("Department ID must be greater than 0");

        RuleFor(x => x.ContactNumber)
            .NotEmpty().WithMessage("Contact number is required")
            .MaximumLength(20).WithMessage("Contact number cannot exceed 20 characters");

        RuleFor(x => x.HireDate)
            .NotEmpty().WithMessage("Hire date is required")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today)).WithMessage("Hire date cannot be in the future");
    }
}

public class UpdateEmployeeDtoValidator : AbstractValidator<UpdateEmployeeDto>
{
    public UpdateEmployeeDtoValidator()
    {
        Include(new CreateEmployeeDtoValidator());
        
        RuleFor(x => x.EmployeeID)
            .GreaterThan(0).WithMessage("Employee ID must be greater than 0");
    }
}

