using HospitalERP.API.Common.Pagination;
using HospitalERP.API.Features.Invoices.Commands;
using HospitalERP.API.Features.Invoices.Dtos;
using HospitalERP.API.Features.Invoices.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalERP.API.Features.Invoices;

[ApiController]
[Route("api/invoices")]
[Authorize(Roles = "Admin,Accountant,Receptionist")]
public class InvoicesController : ControllerBase
{
    private readonly IMediator _mediator;

    public InvoicesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<InvoiceListDto>>> GetAll([FromQuery] QueryParams queryParams)
    {
        var result = await _mediator.Send(new GetAllInvoicesQuery(queryParams));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<InvoiceDetailDto>> GetById(int id)
    {
        var result = await _mediator.Send(new GetInvoiceByIdQuery(id));
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Receptionist")]
    public async Task<ActionResult<InvoiceDetailDto>> Create([FromBody] CreateInvoiceDto dto)
    {
        var result = await _mediator.Send(new CreateInvoiceCommand(dto));
        return CreatedAtAction(nameof(GetById), new { id = result.InvoiceID }, result);
    }

    [HttpPut("{id}/payment-status")]
    [Authorize(Roles = "Admin,Accountant,Receptionist")]
    public async Task<ActionResult<InvoiceDetailDto>> UpdatePaymentStatus(int id, [FromBody] UpdateInvoiceDto dto)
    {
        var command = new UpdateInvoicePaymentStatusCommand(dto with { InvoiceID = id });
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}

