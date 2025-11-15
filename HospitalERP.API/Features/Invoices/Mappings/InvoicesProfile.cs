using AutoMapper;
using HospitalERP.API.Features.Invoices.Dtos;
using HospitalERP.API.Models.Entities;

namespace HospitalERP.API.Features.Invoices.Mappings;

public class InvoicesProfile : Profile
{
    public InvoicesProfile()
    {
        CreateMap<Invoice, InvoiceListDto>()
            .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => $"{src.Patient.FirstName} {src.Patient.LastName}"))
            .ForMember(dest => dest.InvoiceTypeName, opt => opt.MapFrom(src => src.InvoiceType.InvoiceName))
            .ForMember(dest => dest.PaymentStatusName, opt => opt.MapFrom(src => src.PaymentStatus.StatusName));

        CreateMap<Invoice, InvoiceDetailDto>()
            .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => $"{src.Patient.FirstName} {src.Patient.LastName}"))
            .ForMember(dest => dest.InvoiceTypeName, opt => opt.MapFrom(src => src.InvoiceType.InvoiceName))
            .ForMember(dest => dest.PaymentStatusName, opt => opt.MapFrom(src => src.PaymentStatus.StatusName))
            .ForMember(dest => dest.HospitalItems, opt => opt.MapFrom(src => src.HospitalInvoiceItems))
            .ForMember(dest => dest.MedicationItems, opt => opt.MapFrom(src => src.MedicationInvoiceItems));

        CreateMap<HospitalInvoiceItem, HospitalInvoiceItemDto>()
            .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.Service.ServiceName));

        CreateMap<MedicationInvoiceItem, MedicationInvoiceItemDto>()
            .ForMember(dest => dest.MedicationName, opt => opt.MapFrom(src => src.Medication.Name));
    }
}

