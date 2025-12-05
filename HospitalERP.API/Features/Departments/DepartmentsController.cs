using HospitalERP.API.Common.Pagination;
using HospitalERP.API.Features.Departments.Commands;
using HospitalERP.API.Features.Departments.Dtos;
using HospitalERP.API.Features.Departments.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
namespace HospitalERP.API.Features.Departments;

[ApiController]
[Route("api/departments")]
[Authorize(Roles = "Admin")]
[EnableRateLimiting("fixed")]
[ResponseCache(Duration = 60)]
public class DepartmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DepartmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<DepartmentListDto>>> GetAll([FromQuery] QueryParams queryParams)
    {
        var result = await _mediator.Send(new GetAllDepartmentsQuery(queryParams));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DepartmentDetailDto>> GetById(int id)
    {
        var result = await _mediator.Send(new GetDepartmentByIdQuery(id));
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<DepartmentDetailDto>> Create([FromBody] CreateDepartmentDto dto)
    {
        var result = await _mediator.Send(new CreateDepartmentCommand(dto));
        return CreatedAtAction(nameof(GetById), new { id = result.DepartmentID }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<DepartmentDetailDto>> Update(int id, [FromBody] UpdateDepartmentDto dto)
    {
        var command = new UpdateDepartmentCommand(dto with { DepartmentID = id });
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteDepartmentCommand(id));
        return NoContent();
    }
}

