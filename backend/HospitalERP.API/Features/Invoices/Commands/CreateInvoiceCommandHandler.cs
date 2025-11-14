using AutoMapper;
using HospitalERP.API.Common.Exceptions;
using HospitalERP.API.Data;
using HospitalERP.API.Features.Invoices.Dtos;
using HospitalERP.API.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalERP.API.Features.Invoices.Commands;

public class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, InvoiceDetailDto>
{
    private readonly HospitalDbContext _context;
    private readonly IMapper _mapper;

    public CreateInvoiceCommandHandler(HospitalDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<InvoiceDetailDto> Handle(
        CreateInvoiceCommand request,
        CancellationToken cancellationToken)
    {
        // Validate foreign keys
        var patientExists = await _context.Patients
            .AnyAsync(p => p.PatientID == request.Dto.PatientID, cancellationToken);
        if (!patientExists)
        {
            throw new NotFoundException(nameof(Patient), request.Dto.PatientID);
        }

        var invoiceTypeExists = await _context.InvoiceTypes
            .AnyAsync(it => it.InvoiceTypeID == request.Dto.InvoiceTypeID, cancellationToken);
        if (!invoiceTypeExists)
        {
            throw new NotFoundException(nameof(InvoiceType), request.Dto.InvoiceTypeID);
        }

        var paymentStatusExists = await _context.PaymentStatuses
            .AnyAsync(ps => ps.PaymentStatusID == request.Dto.PaymentStatusID, cancellationToken);
        if (!paymentStatusExists)
        {
            throw new NotFoundException(nameof(PaymentStatus), request.Dto.PaymentStatusID);
        }

        // Validate service IDs
        foreach (var item in request.Dto.HospitalItems)
        {
            var serviceExists = await _context.Services
                .AnyAsync(s => s.ServiceID == item.ServiceID, cancellationToken);
            if (!serviceExists)
            {
                throw new NotFoundException(nameof(Service), item.ServiceID);
            }
        }

        // Validate medication IDs and quantities
        foreach (var item in request.Dto.MedicationItems)
        {
            var medicationExists = await _context.Medications
                .AnyAsync(m => m.MedicationID == item.MedicationID, cancellationToken);
            if (!medicationExists)
            {
                throw new NotFoundException(nameof(Medication), item.MedicationID);
            }

            // Check inventory availability
            var inventory = await _context.Inventories
                .FirstOrDefaultAsync(i => i.MedicationID == item.MedicationID, cancellationToken);
            if (inventory == null || inventory.Quantity < item.Quantity)
            {
                throw new BadRequestException($"Insufficient inventory for medication ID {item.MedicationID}");
            }
        }

        // Create invoice
        var invoice = new Invoice
        {
            PatientID = request.Dto.PatientID,
            InvoiceTypeID = request.Dto.InvoiceTypeID,
            InvoiceDate = request.Dto.InvoiceDate,
            PaymentStatusID = request.Dto.PaymentStatusID,
            TotalAmount = 0 // Will be calculated below
        };

        _context.Invoices.Add(invoice);
        await _context.SaveChangesAsync(cancellationToken);

        // Add hospital invoice items
        foreach (var itemDto in request.Dto.HospitalItems)
        {
            var invoiceItem = new HospitalInvoiceItem
            {
                InvoiceID = invoice.InvoiceID,
                ServiceID = itemDto.ServiceID,
                LineTotal = itemDto.LineTotal
            };
            _context.HospitalInvoiceItems.Add(invoiceItem);
            invoice.TotalAmount += itemDto.LineTotal;
        }

        // Add medication invoice items and update inventory
        foreach (var itemDto in request.Dto.MedicationItems)
        {
            var invoiceItem = new MedicationInvoiceItem
            {
                InvoiceID = invoice.InvoiceID,
                MedicationID = itemDto.MedicationID,
                Quantity = itemDto.Quantity,
                LineTotal = itemDto.LineTotal
            };
            _context.MedicationInvoiceItems.Add(invoiceItem);
            invoice.TotalAmount += itemDto.LineTotal;

            // Update inventory quantity
            var inventory = await _context.Inventories
                .FirstAsync(i => i.MedicationID == itemDto.MedicationID, cancellationToken);
            inventory.Quantity -= itemDto.Quantity;
        }

        await _context.SaveChangesAsync(cancellationToken);

        // Reload with all includes
        var createdInvoice = await _context.Invoices
            .Include(i => i.Patient)
            .Include(i => i.InvoiceType)
            .Include(i => i.PaymentStatus)
            .Include(i => i.HospitalInvoiceItems)
                .ThenInclude(hi => hi.Service)
            .Include(i => i.MedicationInvoiceItems)
                .ThenInclude(mi => mi.Medication)
            .FirstAsync(i => i.InvoiceID == invoice.InvoiceID, cancellationToken);

        return _mapper.Map<InvoiceDetailDto>(createdInvoice);
    }
}

