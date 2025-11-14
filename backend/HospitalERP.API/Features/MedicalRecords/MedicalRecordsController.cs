using HospitalERP.API.Common.Pagination;
using HospitalERP.API.Features.MedicalRecords.Commands;
using HospitalERP.API.Features.MedicalRecords.Dtos;
using HospitalERP.API.Features.MedicalRecords.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalERP.API.Features.MedicalRecords;

[ApiController]
[Route("api/medical-records")]
[Authorize(Roles = "Admin,Doctor")]
public class MedicalRecordsController : ControllerBase
{
    private readonly IMediator _mediator;

    public MedicalRecordsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<MedicalRecordListDto>>> GetAll([FromQuery] QueryParams queryParams)
    {
        var result = await _mediator.Send(new GetAllMedicalRecordsQuery(queryParams));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MedicalRecordDetailDto>> GetById(int id)
    {
        var result = await _mediator.Send(new GetMedicalRecordByIdQuery(id));
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<MedicalRecordDetailDto>> Create([FromBody] CreateMedicalRecordDto dto)
    {
        var result = await _mediator.Send(new CreateMedicalRecordCommand(dto));
        return CreatedAtAction(nameof(GetById), new { id = result.RecordID }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MedicalRecordDetailDto>> Update(int id, [FromBody] UpdateMedicalRecordDto dto)
    {
        var command = new UpdateMedicalRecordCommand(dto with { RecordID = id });
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteMedicalRecordCommand(id));
        return NoContent();
    }
}

