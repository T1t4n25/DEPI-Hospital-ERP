using AutoMapper;
using HospitalERP.API.Data;
using HospitalERP.API.Features.Medications.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalERP.API.Features.Medications.Commands;

public class CreateMedicationCommandHandler : IRequestHandler<CreateMedicationCommand, MedicationDetailDto>
{
    private readonly HospitalDbContext _context;
    private readonly IMapper _mapper;

    public CreateMedicationCommandHandler(HospitalDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<MedicationDetailDto> Handle(
        CreateMedicationCommand request,
        CancellationToken cancellationToken)
    {
        var medication = _mapper.Map<Models.Entities.Medication>(request.Dto);
        _context.Medications.Add(medication);
        await _context.SaveChangesAsync(cancellationToken);

        var createdMedication = await _context.Medications
            .Include(m => m.Inventory)
            .FirstAsync(m => m.MedicationID == medication.MedicationID, cancellationToken);

        return _mapper.Map<MedicationDetailDto>(createdMedication);
    }
}

