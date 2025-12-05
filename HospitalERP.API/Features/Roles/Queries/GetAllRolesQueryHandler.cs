using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitalERP.API.Data;
using HospitalERP.API.Models.Entities;

namespace HospitalERP.API.Features.Roles.Queries;

public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, IEnumerable<Role>>
{
    private readonly HospitalDbContext _context;

    public GetAllRolesQueryHandler(HospitalDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Role>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Roles.ToListAsync(cancellationToken);
    }
}
