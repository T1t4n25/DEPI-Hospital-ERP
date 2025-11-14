using HospitalERP.API.Common.Pagination;
using HospitalERP.API.Features.Patients.Commands;
using HospitalERP.API.Features.Patients.Dtos;
using HospitalERP.API.Features.Patients.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalERP.API.Features.Patients;

[ApiController]
[Route("api/patients")]
[Authorize(Roles = "Admin,Doctor,Receptionist")]
public class PatientsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PatientsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<PatientListDto>>> GetAll([FromQuery] QueryParams queryParams)
    {
        var result = await _mediator.Send(new GetAllPatientsQuery(queryParams));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PatientDetailDto>> GetById(int id)
    {
        var result = await _mediator.Send(new GetPatientByIdQuery(id));
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Receptionist")]
    public async Task<ActionResult<PatientDetailDto>> Create([FromBody] CreatePatientDto dto)
    {
        var result = await _mediator.Send(new CreatePatientCommand(dto));
        return CreatedAtAction(nameof(GetById), new { id = result.PatientID }, result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Receptionist")]
    public async Task<ActionResult<PatientDetailDto>> Update(int id, [FromBody] UpdatePatientDto dto)
    {
        var command = new UpdatePatientCommand(dto with { PatientID = id });
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeletePatientCommand(id));
        return NoContent();
    }
}

