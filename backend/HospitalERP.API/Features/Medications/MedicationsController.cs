using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalERP.API.Features.Medications;

[ApiController]
[Route("api/medications")]
[Authorize(Roles = "Admin,Pharmacist")]
public class MedicationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public MedicationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // Endpoints will be added here as Commands/Queries are created
}

