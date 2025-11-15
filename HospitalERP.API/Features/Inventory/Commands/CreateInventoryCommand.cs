using HospitalERP.API.Features.Inventory.Dtos;
using MediatR;

namespace HospitalERP.API.Features.Inventory.Commands;

public record CreateInventoryCommand(CreateInventoryDto Dto) : IRequest<InventoryDetailDto>;

