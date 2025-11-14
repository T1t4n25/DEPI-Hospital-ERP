using HospitalERP.API.Common.Pagination;
using HospitalERP.API.Features.Invoices.Dtos;
using MediatR;

namespace HospitalERP.API.Features.Invoices.Queries;

public record GetAllInvoicesQuery(QueryParams QueryParams) : IRequest<PaginatedResponse<InvoiceListDto>>;

