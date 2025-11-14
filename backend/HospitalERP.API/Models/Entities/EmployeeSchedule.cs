namespace HospitalERP.API.Models.Entities;

public class EmployeeSchedule
{
    public int ScheduleID { get; set; }
    public int EmployeeID { get; set; }
    public TimeOnly ShiftStart { get; set; }
    public TimeOnly ShiftEnd { get; set; }

    // Navigation properties
    public Employee Employee { get; set; } = null!;
}

