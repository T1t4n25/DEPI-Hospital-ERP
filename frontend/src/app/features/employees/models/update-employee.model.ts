import { CreateEmployeeModel } from './create-employee.model';

export interface UpdateEmployeeModel extends CreateEmployeeModel {
  employeeID: number;
}
