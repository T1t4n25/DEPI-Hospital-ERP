using AutoMapper;
using HospitalERP.API.Common.Exceptions;
using HospitalERP.API.Data;
using HospitalERP.API.Features.Invoices.Dtos;
using HospitalERP.API.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalERP.API.Features.Invoices.Queries;

public class GetInvoiceByIdQueryHandler : IRequestHandler<GetInvoiceByIdQuery, InvoiceDetailDto>
{
    private readonly HospitalDbContext _context;
    private readonly IMapper _mapper;

    public GetInvoiceByIdQueryHandler(HospitalDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<InvoiceDetailDto> Handle(
        GetInvoiceByIdQuery request,
        CancellationToken cancellationToken)
    {
        var invoice = await _context.Invoices
            .AsNoTracking()
            .Include(i => i.Patient)
            .Include(i => i.InvoiceType)
            .Include(i => i.PaymentStatus)
            .Include(i => i.HospitalInvoiceItems)
                .ThenInclude(hi => hi.Service)
            .Include(i => i.MedicationInvoiceItems)
                .ThenInclude(mi => mi.Medication)
            .FirstOrDefaultAsync(i => i.InvoiceID == request.InvoiceID, cancellationToken);

        if (invoice == null)
        {
            throw new NotFoundException(nameof(Invoice), request.InvoiceID);
        }

        return _mapper.Map<InvoiceDetailDto>(invoice);
    }
}

