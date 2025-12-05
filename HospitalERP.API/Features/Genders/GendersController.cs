using MediatR;
using Microsoft.AspNetCore.Mvc;
using HospitalERP.API.Features.Genders.Queries;

namespace HospitalERP.API.Features.Genders;

[Route("api/[controller]")]
[ApiController]
public class GendersController : ControllerBase
{
    private readonly IMediator _mediator;

    public GendersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllGendersQuery());
        return Ok(new { data = result });
    }
}
