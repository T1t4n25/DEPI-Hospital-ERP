using HospitalERP.API.Features.MedicalRecords.Dtos;
using MediatR;

namespace HospitalERP.API.Features.MedicalRecords.Commands;

public record CreateMedicalRecordCommand(CreateMedicalRecordDto Dto) : IRequest<MedicalRecordDetailDto>;

