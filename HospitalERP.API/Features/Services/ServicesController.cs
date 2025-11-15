using HospitalERP.API.Common.Pagination;
using HospitalERP.API.Features.Services.Commands;
using HospitalERP.API.Features.Services.Dtos;
using HospitalERP.API.Features.Services.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalERP.API.Features.Services;

[ApiController]
[Route("api/services")]
[Authorize(Roles = "Admin")]
public class ServicesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ServicesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<ServiceListDto>>> GetAll([FromQuery] QueryParams queryParams)
    {
        var result = await _mediator.Send(new GetAllServicesQuery(queryParams));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceDetailDto>> GetById(int id)
    {
        var result = await _mediator.Send(new GetServiceByIdQuery(id));
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceDetailDto>> Create([FromBody] CreateServiceDto dto)
    {
        var result = await _mediator.Send(new CreateServiceCommand(dto));
        return CreatedAtAction(nameof(GetById), new { id = result.ServiceID }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ServiceDetailDto>> Update(int id, [FromBody] UpdateServiceDto dto)
    {
        var command = new UpdateServiceCommand(dto with { ServiceID = id });
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteServiceCommand(id));
        return NoContent();
    }
}

