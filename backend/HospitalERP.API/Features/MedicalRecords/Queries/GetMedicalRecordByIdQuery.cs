using HospitalERP.API.Features.MedicalRecords.Dtos;
using MediatR;

namespace HospitalERP.API.Features.MedicalRecords.Queries;

public record GetMedicalRecordByIdQuery(int RecordID) : IRequest<MedicalRecordDetailDto>;

