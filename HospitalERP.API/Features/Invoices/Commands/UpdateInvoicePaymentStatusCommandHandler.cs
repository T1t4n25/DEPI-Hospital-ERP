using AutoMapper;
using HospitalERP.API.Common.Exceptions;
using HospitalERP.API.Data;
using HospitalERP.API.Features.Invoices.Dtos;
using HospitalERP.API.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalERP.API.Features.Invoices.Commands;

public class UpdateInvoicePaymentStatusCommandHandler : IRequestHandler<UpdateInvoicePaymentStatusCommand, InvoiceDetailDto>
{
    private readonly HospitalDbContext _context;
    private readonly IMapper _mapper;

    public UpdateInvoicePaymentStatusCommandHandler(HospitalDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<InvoiceDetailDto> Handle(
        UpdateInvoicePaymentStatusCommand request,
        CancellationToken cancellationToken)
    {
        var invoice = await _context.Invoices
            .FirstOrDefaultAsync(i => i.InvoiceID == request.Dto.InvoiceID, cancellationToken);

        if (invoice == null)
        {
            throw new NotFoundException(nameof(Invoice), request.Dto.InvoiceID);
        }

        var paymentStatusExists = await _context.PaymentStatuses
            .AnyAsync(ps => ps.PaymentStatusID == request.Dto.PaymentStatusID, cancellationToken);
        if (!paymentStatusExists)
        {
            throw new NotFoundException(nameof(PaymentStatus), request.Dto.PaymentStatusID);
        }

        invoice.PaymentStatusID = request.Dto.PaymentStatusID;
        invoice.PayDate = request.Dto.PayDate;
        await _context.SaveChangesAsync(cancellationToken);

        var updatedInvoice = await _context.Invoices
            .Include(i => i.Patient)
            .Include(i => i.InvoiceType)
            .Include(i => i.PaymentStatus)
            .Include(i => i.HospitalInvoiceItems)
                .ThenInclude(hi => hi.Service)
            .Include(i => i.MedicationInvoiceItems)
                .ThenInclude(mi => mi.Medication)
            .FirstAsync(i => i.InvoiceID == invoice.InvoiceID, cancellationToken);

        return _mapper.Map<InvoiceDetailDto>(updatedInvoice);
    }
}

