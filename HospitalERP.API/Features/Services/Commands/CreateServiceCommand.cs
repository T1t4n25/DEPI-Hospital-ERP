using HospitalERP.API.Features.Services.Dtos;
using MediatR;

namespace HospitalERP.API.Features.Services.Commands;

public record CreateServiceCommand(CreateServiceDto Dto) : IRequest<ServiceDetailDto>;

