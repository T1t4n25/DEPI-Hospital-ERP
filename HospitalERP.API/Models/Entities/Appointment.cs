namespace HospitalERP.API.Models.Entities;

public class Appointment
{
    public int AppointmentID { get; set; }
    public int PatientID { get; set; }
    public int DoctorID { get; set; }
    public int ServiceID { get; set; }
    public DateTime AppointmentDateTime { get; set; }
    public string Status { get; set; } = string.Empty; // Scheduled, Completed, Cancelled

    // Navigation properties
    public Patient Patient { get; set; } = null!;
    public Employee Doctor { get; set; } = null!;
    public Service Service { get; set; } = null!;
}

