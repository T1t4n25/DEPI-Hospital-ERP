import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { SelectModule } from 'primeng/select';
import { RippleModule } from 'primeng/ripple';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { DepartmentsService } from '../services/departments.service';
import { EmployeesService } from '../../employees/services/employees.service';
import { CreateDepartmentModel } from '../models/create-department.model';
import { UpdateDepartmentModel } from '../models/update-department.model';

@Component({
  selector: 'app-department-form',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    CardModule,
    ButtonModule,
    InputTextModule,
    SelectModule,
    RippleModule,
    ToastModule
  ],
  providers: [MessageService],
  templateUrl: './department-form.html',
  styleUrl: './department-form.css'
})
export class DepartmentFormComponent implements OnInit {
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private departmentsService = inject(DepartmentsService);
  private employeesService = inject(EmployeesService);
  private messageService = inject(MessageService);

  isSubmitting = false;
  isEditMode = false;
  isLoading = false;
  departmentId: number | null = null;

  department = {
    departmentName: '',
    managerID: null as number | null | undefined
  };

  managers: any[] = [];

  ngOnInit() {
    this.loadManagers();

    this.route.params.subscribe(params => {
      if (params['id']) {
        this.isEditMode = true;
        this.departmentId = +params['id'];
        this.loadDepartmentData();
      }
    });
  }

  loadManagers() {
    this.employeesService.getAll({ pageSize: 1000 }).subscribe({
      next: (response) => {
        this.managers = response.data.map((e: any) => ({
          label: `${e.firstName} ${e.lastName} (${e.roleName})`,
          value: e.employeeID
        }));
      },
      error: (err) => console.error('Error loading employees:', err)
    });
  }

  loadDepartmentData() {
    if (!this.departmentId) return;

    this.isLoading = true;
    this.departmentsService.getById(this.departmentId).subscribe({
      next: (data) => {
        this.department = {
          departmentName: data.departmentName,
          managerID: data.managerID
        };
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading department:', error);
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to load department data' });
        this.isLoading = false;
      }
    });
  }

  onSubmit() {
    if (this.isSubmitting) return;

    if (!this.department.departmentName) {
      this.messageService.add({ severity: 'warn', summary: 'Validation', detail: 'Please enter department name.' });
      return;
    }

    this.isSubmitting = true;

    if (this.isEditMode && this.departmentId) {
      const model: UpdateDepartmentModel = {
        departmentID: this.departmentId,
        departmentName: this.department.departmentName,
        managerID: this.department.managerID ?? undefined
      };

      this.departmentsService.update(this.departmentId, model).subscribe({
        next: () => {
          this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Department updated successfully' });
          setTimeout(() => this.router.navigate(['/departments']), 1000);
        },
        error: (error) => {
          console.error('Error updating department:', error);
          this.messageService.add({ severity: 'error', summary: 'Error', detail: error.message || 'Failed to update department' });
          this.isSubmitting = false;
        }
      });
    } else {
      const model: CreateDepartmentModel = {
        departmentName: this.department.departmentName,
        managerID: this.department.managerID ?? undefined
      };

      this.departmentsService.create(model).subscribe({
        next: () => {
          this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Department created successfully' });
          setTimeout(() => this.router.navigate(['/departments']), 1000);
        },
        error: (error) => {
          console.error('Error creating department:', error);
          this.messageService.add({ severity: 'error', summary: 'Error', detail: error.message || 'Failed to create department' });
          this.isSubmitting = false;
        }
      });
    }
  }
}
