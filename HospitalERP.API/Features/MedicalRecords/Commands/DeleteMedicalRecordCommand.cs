using MediatR;

namespace HospitalERP.API.Features.MedicalRecords.Commands;

public record DeleteMedicalRecordCommand(int RecordID) : IRequest<Unit>;

