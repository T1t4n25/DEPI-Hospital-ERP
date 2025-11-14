using HospitalERP.API.Common.Pagination;
using HospitalERP.API.Features.Appointments.Commands;
using HospitalERP.API.Features.Appointments.Dtos;
using HospitalERP.API.Features.Appointments.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalERP.API.Features.Appointments;

[ApiController]
[Route("api/appointments")]
[Authorize(Roles = "Admin,Doctor,Receptionist")]
public class AppointmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AppointmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<AppointmentListDto>>> GetAll([FromQuery] QueryParams queryParams)
    {
        var result = await _mediator.Send(new GetAllAppointmentsQuery(queryParams));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AppointmentDetailDto>> GetById(int id)
    {
        var result = await _mediator.Send(new GetAppointmentByIdQuery(id));
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Receptionist")]
    public async Task<ActionResult<AppointmentDetailDto>> Create([FromBody] CreateAppointmentDto dto)
    {
        var result = await _mediator.Send(new CreateAppointmentCommand(dto));
        return CreatedAtAction(nameof(GetById), new { id = result.AppointmentID }, result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Receptionist")]
    public async Task<ActionResult<AppointmentDetailDto>> Update(int id, [FromBody] UpdateAppointmentDto dto)
    {
        var command = new UpdateAppointmentCommand(dto with { AppointmentID = id });
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteAppointmentCommand(id));
        return NoContent();
    }
}

