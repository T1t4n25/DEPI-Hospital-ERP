using MediatR;

namespace HospitalERP.API.Features.Services.Commands;

public record DeleteServiceCommand(int ServiceID) : IRequest<Unit>;

