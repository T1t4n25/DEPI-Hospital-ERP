using MediatR;
using Microsoft.AspNetCore.Mvc;
using HospitalERP.API.Features.Diagnoses.Queries;

namespace HospitalERP.API.Features.Diagnoses;

[Route("api/[controller]")]
[ApiController]
public class DiagnosesController : ControllerBase
{
    private readonly IMediator _mediator;

    public DiagnosesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllDiagnosesQuery());
        return Ok(new { data = result });
    }
}
