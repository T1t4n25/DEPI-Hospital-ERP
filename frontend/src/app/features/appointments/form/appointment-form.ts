import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { DatePickerModule } from 'primeng/datepicker';
import { SelectModule } from 'primeng/select';
import { RippleModule } from 'primeng/ripple';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { AppointmentsService } from '../services/appointments.service';
import { PatientsService } from '../../patients/services/patients.service';
import { CreateAppointmentModel } from '../models/create-appointment.model';
import { UpdateAppointmentModel } from '../models/update-appointment.model';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-appointment-form',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    CardModule,
    ButtonModule,
    DatePickerModule,
    SelectModule,
    RippleModule,
    ToastModule
  ],
  providers: [MessageService],
  templateUrl: './appointment-form.html',
  styleUrl: './appointment-form.css'
})
export class AppointmentFormComponent implements OnInit {
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private appointmentsService = inject(AppointmentsService);
  private patientsService = inject(PatientsService);
  private http = inject(HttpClient);
  private messageService = inject(MessageService);

  isSubmitting = false;
  isEditMode = false;
  isLoading = false;
  appointmentId: number | null = null;

  appointment = {
    patientID: null as number | null,
    doctorID: null as number | null,
    serviceID: null as number | null,
    date: null as Date | null,
    time: null as string | null,
    status: 'Scheduled'
  };

  patients: any[] = [];
  doctors: any[] = [];
  services: any[] = [];

  timeSlots = [
    '09:00', '09:30', '10:00', '10:30', '11:00', '11:30',
    '12:00', '12:30', '13:00', '13:30', '14:00', '14:30',
    '15:00', '15:30', '16:00', '16:30', '17:00'
  ].map(time => ({ label: this.formatTime(time), value: time }));

  ngOnInit() {
    this.loadDropdownData();

    this.route.params.subscribe(params => {
      if (params['id']) {
        this.isEditMode = true;
        this.appointmentId = +params['id'];
        this.loadAppointmentData();
      }
    });
  }

  formatTime(time: string): string {
    const [hours, minutes] = time.split(':');
    const hour = parseInt(hours);
    const ampm = hour >= 12 ? 'PM' : 'AM';
    const displayHour = hour > 12 ? hour - 12 : hour === 0 ? 12 : hour;
    return `${displayHour}:${minutes} ${ampm}`;
  }

  loadDropdownData() {
    // Load patients
    this.patientsService.getAll({ pageSize: 1000 }).subscribe({
      next: (response) => {
        this.patients = response.data.map((p: any) => ({
          label: `${p.firstName} ${p.lastName}`,
          value: p.patientID
        }));
      },
      error: (err) => console.error('Error loading patients:', err)
    });

    // Load doctors (employees with Doctor role)
    this.http.get<any>(`${environment.apiUrl}/api/employees`, {
      params: { pageSize: '1000' }
    }).subscribe({
      next: (response) => {
        this.doctors = response.data
          .filter((e: any) => e.roleName === 'Doctor')
          .map((e: any) => ({
            label: `Dr. ${e.firstName} ${e.lastName}`,
            value: e.employeeID
          }));
      },
      error: (err) => console.error('Error loading doctors:', err)
    });

    // Load services
    this.http.get<any>(`${environment.apiUrl}/api/services`, {
      params: { pageSize: '1000' }
    }).subscribe({
      next: (response) => {
        this.services = response.data.map((s: any) => ({
          label: s.serviceName,
          value: s.serviceID
        }));
      },
      error: (err) => console.error('Error loading services:', err)
    });
  }

  loadAppointmentData() {
    if (!this.appointmentId) return;

    this.isLoading = true;
    this.appointmentsService.getById(this.appointmentId).subscribe({
      next: (data) => {
        const appointmentDate = new Date(data.appointmentDateTime);
        this.appointment = {
          patientID: data.patientID,
          doctorID: data.doctorID,
          serviceID: data.serviceID,
          date: appointmentDate,
          time: `${appointmentDate.getHours().toString().padStart(2, '0')}:${appointmentDate.getMinutes().toString().padStart(2, '0')}`,
          status: data.status
        };
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading appointment:', error);
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to load appointment data' });
        this.isLoading = false;
      }
    });
  }

  onSubmit() {
    if (this.isSubmitting) return;

    // Validation
    if (!this.appointment.patientID || !this.appointment.doctorID || !this.appointment.serviceID ||
      !this.appointment.date || !this.appointment.time) {
      this.messageService.add({ severity: 'warn', summary: 'Validation', detail: 'Please fill in all required fields.' });
      return;
    }

    this.isSubmitting = true;

    // Combine date and time into ISO datetime
    const [hours, minutes] = this.appointment.time.split(':');
    const appointmentDateTime = new Date(this.appointment.date);
    appointmentDateTime.setHours(parseInt(hours), parseInt(minutes), 0, 0);

    if (this.isEditMode && this.appointmentId) {
      // Update existing appointment
      const model: UpdateAppointmentModel = {
        appointmentID: this.appointmentId,
        patientID: this.appointment.patientID,
        doctorID: this.appointment.doctorID,
        serviceID: this.appointment.serviceID,
        appointmentDateTime: appointmentDateTime.toISOString(),
        status: this.appointment.status
      };

      this.appointmentsService.update(this.appointmentId, model).subscribe({
        next: () => {
          this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Appointment updated successfully' });
          setTimeout(() => this.router.navigate(['/appointments']), 1000);
        },
        error: (error) => {
          console.error('Error updating appointment:', error);
          this.messageService.add({ severity: 'error', summary: 'Error', detail: error.message || 'Failed to update appointment' });
          this.isSubmitting = false;
        }
      });
    } else {
      // Create new appointment
      const model: CreateAppointmentModel = {
        patientID: this.appointment.patientID,
        doctorID: this.appointment.doctorID,
        serviceID: this.appointment.serviceID,
        appointmentDateTime: appointmentDateTime.toISOString(),
        status: this.appointment.status
      };

      this.appointmentsService.create(model).subscribe({
        next: () => {
          this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Appointment created successfully' });
          setTimeout(() => this.router.navigate(['/appointments']), 1000);
        },
        error: (error) => {
          console.error('Error creating appointment:', error);
          this.messageService.add({ severity: 'error', summary: 'Error', detail: error.message || 'Failed to create appointment' });
          this.isSubmitting = false;
        }
      });
    }
  }
}
