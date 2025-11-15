using HospitalERP.API.Common.Pagination;
using HospitalERP.API.Features.Medications.Commands;
using HospitalERP.API.Features.Medications.Dtos;
using HospitalERP.API.Features.Medications.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalERP.API.Features.Medications;

[ApiController]
[Route("api/medications")]
[Authorize(Roles = "Admin,Pharmacist")]
public class MedicationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public MedicationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<MedicationListDto>>> GetAll([FromQuery] QueryParams queryParams)
    {
        var result = await _mediator.Send(new GetAllMedicationsQuery(queryParams));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MedicationDetailDto>> GetById(int id)
    {
        var result = await _mediator.Send(new GetMedicationByIdQuery(id));
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<MedicationDetailDto>> Create([FromBody] CreateMedicationDto dto)
    {
        var result = await _mediator.Send(new CreateMedicationCommand(dto));
        return CreatedAtAction(nameof(GetById), new { id = result.MedicationID }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MedicationDetailDto>> Update(int id, [FromBody] UpdateMedicationDto dto)
    {
        var command = new UpdateMedicationCommand(dto with { MedicationID = id });
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteMedicationCommand(id));
        return NoContent();
    }
}

