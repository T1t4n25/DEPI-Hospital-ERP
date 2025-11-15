using AutoMapper;
using HospitalERP.API.Common.Exceptions;
using HospitalERP.API.Data;
using HospitalERP.API.Features.Medications.Dtos;
using HospitalERP.API.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalERP.API.Features.Medications.Commands;

public class UpdateMedicationCommandHandler : IRequestHandler<UpdateMedicationCommand, MedicationDetailDto>
{
    private readonly HospitalDbContext _context;
    private readonly IMapper _mapper;

    public UpdateMedicationCommandHandler(HospitalDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<MedicationDetailDto> Handle(
        UpdateMedicationCommand request,
        CancellationToken cancellationToken)
    {
        var medication = await _context.Medications
            .FirstOrDefaultAsync(m => m.MedicationID == request.Dto.MedicationID, cancellationToken);

        if (medication == null)
        {
            throw new NotFoundException(nameof(Medication), request.Dto.MedicationID);
        }

        _mapper.Map(request.Dto, medication);
        await _context.SaveChangesAsync(cancellationToken);

        var updatedMedication = await _context.Medications
            .Include(m => m.Inventory)
            .FirstAsync(m => m.MedicationID == medication.MedicationID, cancellationToken);

        return _mapper.Map<MedicationDetailDto>(updatedMedication);
    }
}

