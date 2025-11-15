using AutoMapper;
using HospitalERP.API.Common.Exceptions;
using HospitalERP.API.Data;
using HospitalERP.API.Features.Medications.Dtos;
using HospitalERP.API.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalERP.API.Features.Medications.Queries;

public class GetMedicationByIdQueryHandler : IRequestHandler<GetMedicationByIdQuery, MedicationDetailDto>
{
    private readonly HospitalDbContext _context;
    private readonly IMapper _mapper;

    public GetMedicationByIdQueryHandler(HospitalDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<MedicationDetailDto> Handle(
        GetMedicationByIdQuery request,
        CancellationToken cancellationToken)
    {
        var medication = await _context.Medications
            .AsNoTracking()
            .Include(m => m.Inventory)
            .FirstOrDefaultAsync(m => m.MedicationID == request.MedicationID, cancellationToken);

        if (medication == null)
        {
            throw new NotFoundException(nameof(Medication), request.MedicationID);
        }

        return _mapper.Map<MedicationDetailDto>(medication);
    }
}

