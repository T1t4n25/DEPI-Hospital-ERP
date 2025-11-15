using HospitalERP.API.Features.Inventory.Dtos;
using MediatR;

namespace HospitalERP.API.Features.Inventory.Queries;

public record GetInventoryByIdQuery(int MedicationID) : IRequest<InventoryDetailDto>;

