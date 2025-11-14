namespace HospitalERP.API.Models.Entities;

public class Service
{
    public int ServiceID { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public decimal Cost { get; set; }
    public int DepartmentID { get; set; }

    // Navigation properties
    public Department Department { get; set; } = null!;
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<HospitalInvoiceItem> HospitalInvoiceItems { get; set; } = new List<HospitalInvoiceItem>();
}

