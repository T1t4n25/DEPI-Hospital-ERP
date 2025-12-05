using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitalERP.API.Data;
using HospitalERP.API.Models.Entities;

namespace HospitalERP.API.Features.Diagnoses.Queries;

public class GetAllDiagnosesQueryHandler : IRequestHandler<GetAllDiagnosesQuery, IEnumerable<Diagnosis>>
{
    private readonly HospitalDbContext _context;

    public GetAllDiagnosesQueryHandler(HospitalDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Diagnosis>> Handle(GetAllDiagnosesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Diagnoses.ToListAsync(cancellationToken);
    }
}
