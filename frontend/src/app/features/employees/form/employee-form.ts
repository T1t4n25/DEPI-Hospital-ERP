import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { DatePickerModule } from 'primeng/datepicker';
import { SelectModule } from 'primeng/select';
import { RippleModule } from 'primeng/ripple';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { EmployeesService } from '../services/employees.service';
import { DepartmentsService } from '../../departments/services/departments.service';
import { CreateEmployeeModel } from '../models/create-employee.model';
import { UpdateEmployeeModel } from '../models/update-employee.model';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-employee-form',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    CardModule,
    ButtonModule,
    InputTextModule,
    DatePickerModule,
    SelectModule,
    RippleModule,
    ToastModule
  ],
  providers: [MessageService],
  templateUrl: './employee-form.html',
  styleUrl: './employee-form.css'
})
export class EmployeeFormComponent implements OnInit {
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private employeesService = inject(EmployeesService);
  private departmentsService = inject(DepartmentsService);
  private http = inject(HttpClient);
  private messageService = inject(MessageService);

  isSubmitting = false;
  isEditMode = false;
  isLoading = false;
  employeeId: number | null = null;

  employee = {
    firstName: '',
    lastName: '',
    genderID: null as number | null,
    roleID: null as number | null,
    departmentID: null as number | null,
    contactNumber: '',
    hireDate: null as Date | null
  };

  genders: any[] = [];
  roles: any[] = [];
  departments: any[] = [];

  ngOnInit() {
    this.loadDropdownData();

    this.route.params.subscribe(params => {
      if (params['id']) {
        this.isEditMode = true;
        this.employeeId = +params['id'];
        this.loadEmployeeData();
      }
    });
  }

  loadDropdownData() {
    // Load genders
    this.http.get<any>(`${environment.apiUrl}/api/genders`).subscribe({
      next: (response) => {
        this.genders = response.data.map((g: any) => ({
          label: g.genderName,
          value: g.genderID
        }));
      },
      error: (err) => console.error('Error loading genders:', err)
    });

    // Load roles
    this.http.get<any>(`${environment.apiUrl}/api/roles`).subscribe({
      next: (response) => {
        this.roles = response.data.map((r: any) => ({
          label: r.roleName,
          value: r.roleID
        }));
      },
      error: (err) => console.error('Error loading roles:', err)
    });

    // Load departments
    this.departmentsService.getAll({ pageSize: 1000 }).subscribe({
      next: (response) => {
        this.departments = response.data.map((d: any) => ({
          label: d.departmentName,
          value: d.departmentID
        }));
      },
      error: (err) => console.error('Error loading departments:', err)
    });
  }

  loadEmployeeData() {
    if (!this.employeeId) return;

    this.isLoading = true;
    this.employeesService.getById(this.employeeId).subscribe({
      next: (data) => {
        this.employee = {
          firstName: data.firstName,
          lastName: data.lastName,
          genderID: data.genderID,
          roleID: data.roleID,
          departmentID: data.departmentID,
          contactNumber: data.contactNumber,
          hireDate: new Date(data.hireDate)
        };
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading employee:', error);
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to load employee data' });
        this.isLoading = false;
      }
    });
  }

  onSubmit() {
    if (this.isSubmitting) return;

    // Validation
    if (!this.employee.firstName || !this.employee.lastName || !this.employee.genderID ||
      !this.employee.roleID || !this.employee.departmentID || !this.employee.contactNumber ||
      !this.employee.hireDate) {
      this.messageService.add({ severity: 'warn', summary: 'Validation', detail: 'Please fill in all required fields.' });
      return;
    }

    this.isSubmitting = true;

    // Format date to YYYY-MM-DD
    const formattedDate = this.employee.hireDate ?
      new Date(this.employee.hireDate).toISOString().split('T')[0] : '';

    if (this.isEditMode && this.employeeId) {
      // Update existing employee
      const model: UpdateEmployeeModel = {
        employeeID: this.employeeId,
        firstName: this.employee.firstName,
        lastName: this.employee.lastName,
        genderID: this.employee.genderID,
        roleID: this.employee.roleID,
        departmentID: this.employee.departmentID,
        contactNumber: this.employee.contactNumber,
        hireDate: formattedDate
      };

      this.employeesService.update(this.employeeId, model).subscribe({
        next: () => {
          this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Employee updated successfully' });
          setTimeout(() => this.router.navigate(['/employees']), 1000);
        },
        error: (error) => {
          console.error('Error updating employee:', error);
          this.messageService.add({ severity: 'error', summary: 'Error', detail: error.message || 'Failed to update employee' });
          this.isSubmitting = false;
        }
      });
    } else {
      // Create new employee
      const model: CreateEmployeeModel = {
        firstName: this.employee.firstName,
        lastName: this.employee.lastName,
        genderID: this.employee.genderID,
        roleID: this.employee.roleID,
        departmentID: this.employee.departmentID,
        contactNumber: this.employee.contactNumber,
        hireDate: formattedDate
      };

      this.employeesService.create(model).subscribe({
        next: () => {
          this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Employee created successfully' });
          setTimeout(() => this.router.navigate(['/employees']), 1000);
        },
        error: (error) => {
          console.error('Error creating employee:', error);
          this.messageService.add({ severity: 'error', summary: 'Error', detail: error.message || 'Failed to create employee' });
          this.isSubmitting = false;
        }
      });
    }
  }
}
