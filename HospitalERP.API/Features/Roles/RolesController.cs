using MediatR;
using Microsoft.AspNetCore.Mvc;
using HospitalERP.API.Features.Roles.Queries;

namespace HospitalERP.API.Features.Roles;

[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RolesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllRolesQuery());
        return Ok(new { data = result });
    }
}
