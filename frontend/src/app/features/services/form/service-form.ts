import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { InputNumberModule } from 'primeng/inputnumber';
import { SelectModule } from 'primeng/select';
import { RippleModule } from 'primeng/ripple';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { ServicesService } from '../services/services.service';
import { DepartmentsService } from '../../departments/services/departments.service';
import { CreateServiceModel } from '../models/create-service.model';
import { UpdateServiceModel } from '../models/update-service.model';

@Component({
  selector: 'app-service-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, CardModule, ButtonModule, InputTextModule, InputNumberModule, SelectModule, RippleModule, ToastModule],
  providers: [MessageService],
  templateUrl: './service-form.html',
  styleUrl: './service-form.css'
})
export class ServiceFormComponent implements OnInit {
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private servicesService = inject(ServicesService);
  private departmentsService = inject(DepartmentsService);
  private messageService = inject(MessageService);

  isSubmitting = false;
  isEditMode = false;
  isLoading = false;
  serviceId: number | null = null;

  service = {
    name: '',
    cost: null as number | null,
    department: null as number | null
  };

  departments: any[] = [];

  ngOnInit() {
    this.loadDropdownData();

    this.route.params.subscribe(params => {
      if (params['id']) {
        this.isEditMode = true;
        this.serviceId = +params['id'];
        this.loadServiceData();
      }
    });
  }

  loadDropdownData() {
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

  loadServiceData() {
    if (!this.serviceId) return;

    this.isLoading = true;
    this.servicesService.getById(this.serviceId).subscribe({
      next: (data) => {
        this.service = {
          name: data.serviceName,
          cost: data.cost,
          department: data.departmentID
        };
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading service:', error);
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to load service data' });
        this.isLoading = false;
      }
    });
  }

  onSubmit() {
    if (this.isSubmitting) return;

    // Validation
    if (!this.service.name || !this.service.cost || !this.service.department) {
      this.messageService.add({ severity: 'warn', summary: 'Validation', detail: 'Please fill in all required fields.' });
      return;
    }

    this.isSubmitting = true;

    if (this.isEditMode && this.serviceId) {
      // Update existing service
      const model: UpdateServiceModel = {
        serviceID: this.serviceId,
        serviceName: this.service.name,
        cost: this.service.cost,
        departmentID: this.service.department
      };

      this.servicesService.update(this.serviceId, model).subscribe({
        next: () => {
          this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Service updated successfully' });
          setTimeout(() => this.router.navigate(['/services']), 1000);
        },
        error: (error) => {
          console.error('Error updating service:', error);
          this.messageService.add({ severity: 'error', summary: 'Error', detail: error.message || 'Failed to update service' });
          this.isSubmitting = false;
        }
      });
    } else {
      // Create new service
      const model: CreateServiceModel = {
        serviceName: this.service.name,
        cost: this.service.cost,
        departmentID: this.service.department
      };

      this.servicesService.create(model).subscribe({
        next: () => {
          this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Service created successfully' });
          setTimeout(() => this.router.navigate(['/services']), 1000);
        },
        error: (error) => {
          console.error('Error creating service:', error);
          this.messageService.add({ severity: 'error', summary: 'Error', detail: error.message || 'Failed to create service' });
          this.isSubmitting = false;
        }
      });
    }
  }
}

