using HospitalERP.API.Features.Patients.Dtos;
using MediatR;

namespace HospitalERP.API.Features.Patients.Commands;

public record CreatePatientCommand(CreatePatientDto Dto) : IRequest<PatientDetailDto>;

