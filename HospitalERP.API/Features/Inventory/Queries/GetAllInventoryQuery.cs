using HospitalERP.API.Common.Pagination;
using HospitalERP.API.Features.Inventory.Dtos;
using MediatR;

namespace HospitalERP.API.Features.Inventory.Queries;

public record GetAllInventoryQuery(QueryParams QueryParams) : IRequest<PaginatedResponse<InventoryListDto>>;

