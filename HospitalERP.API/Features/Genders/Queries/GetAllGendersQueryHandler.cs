using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitalERP.API.Data;
using HospitalERP.API.Models.Entities;

namespace HospitalERP.API.Features.Genders.Queries;

public class GetAllGendersQueryHandler : IRequestHandler<GetAllGendersQuery, IEnumerable<Gender>>
{
    private readonly HospitalDbContext _context;

    public GetAllGendersQueryHandler(HospitalDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Gender>> Handle(GetAllGendersQuery request, CancellationToken cancellationToken)
    {
        return await _context.Genders.ToListAsync(cancellationToken);
    }
}
