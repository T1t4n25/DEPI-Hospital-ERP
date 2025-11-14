using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalERP.API.Features.Patients;

[ApiController]
[Route("api/patients")]
[Authorize(Roles = "Admin,Doctor,Receptionist")]
public class PatientsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PatientsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // Endpoints will be added here as Commands/Queries are created
}

