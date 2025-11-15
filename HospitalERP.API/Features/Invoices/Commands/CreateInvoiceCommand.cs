using HospitalERP.API.Features.Invoices.Dtos;
using MediatR;

namespace HospitalERP.API.Features.Invoices.Commands;

public record CreateInvoiceCommand(CreateInvoiceDto Dto) : IRequest<InvoiceDetailDto>;

