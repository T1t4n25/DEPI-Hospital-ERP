using HospitalERP.API.Features.Patients.Dtos;
using MediatR;

namespace HospitalERP.API.Features.Patients.Commands;

public record UpdatePatientCommand(UpdatePatientDto Dto) : IRequest<PatientDetailDto>;

