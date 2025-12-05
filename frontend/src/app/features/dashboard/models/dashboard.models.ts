// Admin Dashboard Models
export interface AdminDashboardDto {
  totalPatients: number;
  todayAppointments: number;
  totalRevenue: number;
  activeEmployees: number;
  totalDepartments: number;
  totalServices: number;
  pendingInvoices: number;
  weeklyAppointments: WeeklyAppointmentStatDto[];
  monthlyRevenue: MonthlyRevenueDto[];
  departmentStats: DepartmentStatDto[];
  recentActivities: RecentActivityDto[];
}

export interface WeeklyAppointmentStatDto {
  day: string;
  count: number;
}

export interface MonthlyRevenueDto {
  month: string;
  amount: number;
}

export interface DepartmentStatDto {
  departmentName: string;
  employeeCount: number;
}

export interface RecentActivityDto {
  type: string;
  description: string;
  timestamp: string;
}

// HR Dashboard Models
export interface HrDashboardDto {
  totalEmployees: number;
  presentToday: number;
  onLeave: number;
  totalDepartments: number;
  departmentEmployeeCounts: DepartmentEmployeeCountDto[];
  roleCounts: RoleCountDto[];
  recentHires: RecentHireDto[];
}

export interface DepartmentEmployeeCountDto {
  departmentName: string;
  employeeCount: number;
}

export interface RoleCountDto {
  roleName: string;
  count: number;
}

export interface RecentHireDto {
  employeeID: number;
  fullName: string;
  roleName: string;
  departmentName: string;
  hireDate: string;
}

// Accountant Dashboard Models
export interface AccountantDashboardDto {
  totalRevenue: number;
  pendingPayments: number;
  paidAmount: number;
  totalInvoices: number;
  paidInvoices: number;
  pendingInvoices: number;
  monthlyRevenue: MonthlyRevenueDto[];
  invoiceStatusBreakdown: InvoiceStatusDto[];
  recentInvoices: RecentInvoiceDto[];
}

export interface InvoiceStatusDto {
  statusName: string;
  count: number;
  totalAmount: number;
}

export interface RecentInvoiceDto {
  invoiceID: number;
  patientName: string;
  totalAmount: number;
  paymentStatusName: string;
  invoiceDate: string;
}

