using MediatR;

namespace HospitalERP.API.Features.Patients.Commands;

public record DeletePatientCommand(int PatientID) : IRequest<Unit>;

