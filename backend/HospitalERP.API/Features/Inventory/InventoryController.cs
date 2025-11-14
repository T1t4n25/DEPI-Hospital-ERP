using HospitalERP.API.Common.Pagination;
using HospitalERP.API.Features.Inventory.Commands;
using HospitalERP.API.Features.Inventory.Dtos;
using HospitalERP.API.Features.Inventory.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalERP.API.Features.Inventory;

[ApiController]
[Route("api/inventory")]
[Authorize(Roles = "Admin,Pharmacist")]
public class InventoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public InventoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<InventoryListDto>>> GetAll([FromQuery] QueryParams queryParams)
    {
        var result = await _mediator.Send(new GetAllInventoryQuery(queryParams));
        return Ok(result);
    }

    [HttpGet("{medicationId}")]
    public async Task<ActionResult<InventoryDetailDto>> GetById(int medicationId)
    {
        var result = await _mediator.Send(new GetInventoryByIdQuery(medicationId));
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<InventoryDetailDto>> Create([FromBody] CreateInventoryDto dto)
    {
        var result = await _mediator.Send(new CreateInventoryCommand(dto));
        return CreatedAtAction(nameof(GetById), new { medicationId = result.MedicationID }, result);
    }

    [HttpPut("{medicationId}")]
    public async Task<ActionResult<InventoryDetailDto>> Update(int medicationId, [FromBody] UpdateInventoryDto dto)
    {
        var command = new UpdateInventoryCommand(dto with { MedicationID = medicationId });
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}

