using HospitalERP.API.Common.Pagination;
using HospitalERP.API.Features.Employees.Commands;
using HospitalERP.API.Features.Employees.Dtos;
using HospitalERP.API.Features.Employees.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalERP.API.Features.Employees;

[ApiController]
[Route("api/employees")]
[Authorize(Roles = "Admin,HR")]
public class EmployeesController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployeesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<EmployeeListDto>>> GetAll([FromQuery] QueryParams queryParams)
    {
        var result = await _mediator.Send(new GetAllEmployeesQuery(queryParams));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeDetailDto>> GetById(int id)
    {
        var result = await _mediator.Send(new GetEmployeeByIdQuery(id));
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<EmployeeDetailDto>> Create([FromBody] CreateEmployeeDto dto)
    {
        var result = await _mediator.Send(new CreateEmployeeCommand(dto));
        return CreatedAtAction(nameof(GetById), new { id = result.EmployeeID }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<EmployeeDetailDto>> Update(int id, [FromBody] UpdateEmployeeDto dto)
    {
        var command = new UpdateEmployeeCommand(dto with { EmployeeID = id });
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteEmployeeCommand(id));
        return NoContent();
    }
}

