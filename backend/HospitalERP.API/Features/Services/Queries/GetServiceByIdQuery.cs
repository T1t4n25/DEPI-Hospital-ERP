using HospitalERP.API.Features.Services.Dtos;
using MediatR;

namespace HospitalERP.API.Features.Services.Queries;

public record GetServiceByIdQuery(int ServiceID) : IRequest<ServiceDetailDto>;

