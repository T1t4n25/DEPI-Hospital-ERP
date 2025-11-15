using HospitalERP.API.Features.Invoices.Dtos;
using MediatR;

namespace HospitalERP.API.Features.Invoices.Queries;

public record GetInvoiceByIdQuery(int InvoiceID) : IRequest<InvoiceDetailDto>;

