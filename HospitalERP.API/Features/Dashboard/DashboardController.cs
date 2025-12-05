using HospitalERP.API.Features.Dashboard.Dtos;
using HospitalERP.API.Features.Dashboard.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalERP.API.Features.Dashboard;

[ApiController]
[Route("api/dashboard")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public DashboardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<AdminDashboardDto>> GetAdminDashboard()
    {
        var result = await _mediator.Send(new GetAdminDashboardQuery());
        return Ok(result);
    }

    [HttpGet("hr")]
    [Authorize(Roles = "Admin,HR")]
    public async Task<ActionResult<HrDashboardDto>> GetHrDashboard()
    {
        var result = await _mediator.Send(new GetHrDashboardQuery());
        return Ok(result);
    }

    [HttpGet("accountant")]
    [Authorize(Roles = "Admin,Accountant")]
    public async Task<ActionResult<AccountantDashboardDto>> GetAccountantDashboard()
    {
        var result = await _mediator.Send(new GetAccountantDashboardQuery());
        return Ok(result);
    }
}

