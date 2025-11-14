using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalERP.API.Features.Appointments;

[ApiController]
[Route("api/appointments")]
[Authorize(Roles = "Admin,Doctor,Receptionist")]
public class AppointmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AppointmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // Endpoints will be added here as Commands/Queries are created
}

