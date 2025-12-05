import { Component, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { TagModule } from 'primeng/tag';
import { RippleModule } from 'primeng/ripple';
import { TooltipModule } from 'primeng/tooltip';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { MessageService, ConfirmationService } from 'primeng/api';
import { AppointmentsService } from '../services/appointments.service';
import { AppointmentListModel } from '../models/appointment-list.model';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { catchError, of } from 'rxjs';
import { LoadingSpinnerComponent } from '../../../shared/components/loading-spinner/loading-spinner.component';

@Component({
  selector: 'app-appointments-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    TableModule,
    ButtonModule,
    InputTextModule,
    TagModule,
    RippleModule,
    TooltipModule,
    ToastModule,
    ConfirmDialogModule,
    LoadingSpinnerComponent
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './appointments-list.html',
  styleUrl: './appointments-list.css'
})
export class AppointmentsListComponent {
  private readonly service = inject(AppointmentsService);
  private readonly messageService = inject(MessageService);
  private readonly confirmationService = inject(ConfirmationService);

  readonly loading = signal<boolean>(false);
  readonly error = signal<string | null>(null);
  readonly appointments = signal<AppointmentListModel[]>([]);
  readonly searchTerm = signal<string>('');

  readonly filteredAppointments = computed(() => {
    const items = this.appointments();
    const term = this.searchTerm().toLowerCase();
    if (!term) return items;

    return items.filter(a =>
      a.patientName.toLowerCase().includes(term) ||
      a.doctorName.toLowerCase().includes(term) ||
      a.serviceName.toLowerCase().includes(term)
    );
  });

  constructor() {
    this.loadAppointments();
  }

  loadAppointments() {
    this.loading.set(true);
    this.error.set(null);

    this.service.getAll({ pageSize: 10000 })
      .pipe(
        takeUntilDestroyed(),
        catchError((err: Error) => {
          this.error.set(err.message);
          return of({ data: [], pageNumber: 1, pageSize: 10, totalCount: 0, totalPages: 0, hasPreviousPage: false, hasNextPage: false });
        })
      )
      .subscribe({
        next: (response: any) => {
          this.appointments.set(response.data);
          this.loading.set(false);
        },
        error: () => this.loading.set(false)
      });
  }

  deleteAppointment(appointment: AppointmentListModel) {
    this.confirmationService.confirm({
      message: `Are you sure you want to delete the appointment for ${appointment.patientName} with ${appointment.doctorName}?`,
      header: 'Confirm Deletion',
      icon: 'pi pi-exclamation-triangle',
      acceptButtonStyleClass: 'p-button-danger',
      accept: () => {
        this.loading.set(true);
        this.service.delete(appointment.appointmentID)
          .subscribe({
            next: () => {
              this.messageService.add({
                severity: 'success',
                summary: 'Success',
                detail: 'Appointment deleted successfully'
              });
              this.loadAppointments();
            },
            error: (err: Error) => {
              this.messageService.add({
                severity: 'error',
                summary: 'Error',
                detail: err.message || 'Failed to delete appointment'
              });
              this.loading.set(false);
            }
          });
      }
    });
  }

  getStatusSeverity(status: string) {
    switch (status) {
      case 'Scheduled': return 'info';
      case 'Completed': return 'success';
      case 'Cancelled': return 'danger';
      default: return 'warn';
    }
  }
}
