using HospitalERP.API.Features.Services.Dtos;
using MediatR;

namespace HospitalERP.API.Features.Services.Commands;

public record UpdateServiceCommand(UpdateServiceDto Dto) : IRequest<ServiceDetailDto>;

