export interface EmployeeDetailModel {
  employeeID: number;
  firstName: string;
  lastName: string;
  genderID: number;
  genderName: string;
  roleID: number;
  roleName: string;
  departmentID: number;
  departmentName: string;
  contactNumber: string;
  hireDate: string; // ISO date string (YYYY-MM-DD)
}
