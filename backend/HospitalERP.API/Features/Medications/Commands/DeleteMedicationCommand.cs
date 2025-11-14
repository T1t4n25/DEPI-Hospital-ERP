using MediatR;

namespace HospitalERP.API.Features.Medications.Commands;

public record DeleteMedicationCommand(int MedicationID) : IRequest<Unit>;

