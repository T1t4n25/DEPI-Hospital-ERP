using HospitalERP.API.Features.Patients.Dtos;
using MediatR;

namespace HospitalERP.API.Features.Patients.Queries;

public record GetPatientByIdQuery(int PatientID) : IRequest<PatientDetailDto>;

