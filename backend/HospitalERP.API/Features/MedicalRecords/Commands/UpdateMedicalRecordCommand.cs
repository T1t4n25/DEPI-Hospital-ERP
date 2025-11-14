using HospitalERP.API.Features.MedicalRecords.Dtos;
using MediatR;

namespace HospitalERP.API.Features.MedicalRecords.Commands;

public record UpdateMedicalRecordCommand(UpdateMedicalRecordDto Dto) : IRequest<MedicalRecordDetailDto>;

