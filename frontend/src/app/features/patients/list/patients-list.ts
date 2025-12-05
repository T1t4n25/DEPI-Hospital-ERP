import { Component, inject, signal, computed, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { TagModule } from 'primeng/tag';
import { RippleModule } from 'primeng/ripple';
import { TooltipModule } from 'primeng/tooltip';
import { ConfirmationService, MessageService } from 'primeng/api';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ToastModule } from 'primeng/toast';
import { PatientsService } from '../services/patients.service';
import { PatientListModel } from '../models/patient-list.model';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { catchError, of } from 'rxjs';
import { LoadingSpinnerComponent } from '../../../shared/components/loading-spinner/loading-spinner.component';

@Component({
  selector: 'app-patients-list',
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
    ConfirmDialogModule,
    ToastModule,
    LoadingSpinnerComponent
  ],
  templateUrl: './patients-list.html',
  styleUrl: './patients-list.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [ConfirmationService, MessageService]
})
export class PatientsListComponent {
  private readonly service = inject(PatientsService);
  private readonly confirmationService = inject(ConfirmationService);
  private readonly messageService = inject(MessageService);

  readonly loading = signal<boolean>(false);
  readonly error = signal<string | null>(null);
  readonly patients = signal<PatientListModel[]>([]);
  readonly searchTerm = signal<string>('');

  readonly filteredPatients = computed(() => {
    const items = this.patients();
    const term = this.searchTerm().toLowerCase();
    if (!term) return items;

    return items.filter(p =>
      p.firstName.toLowerCase().includes(term) ||
      p.lastName.toLowerCase().includes(term) ||
      p.contactNumber.includes(term)
    );
  });

  constructor() {
    this.loadPatients();
  }

  loadPatients() {
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
          this.patients.set(response.data);
          this.loading.set(false);
        },
        error: () => this.loading.set(false)
      });
  }

  deletePatient(patient: PatientListModel) {
    this.confirmationService.confirm({
      message: `Are you sure you want to delete ${patient.firstName} ${patient.lastName}?`,
      header: 'Confirm Deletion',
      icon: 'pi pi-exclamation-triangle',
      acceptButtonStyleClass: 'p-button-danger',
      accept: () => {
        this.loading.set(true);
        this.service.delete(patient.patientID)
          .subscribe({
            next: () => {
              this.messageService.add({
                severity: 'success',
                summary: 'Success',
                detail: 'Patient deleted successfully'
              });
              this.loadPatients();
            },
            error: (err: Error) => {
              this.messageService.add({
                severity: 'error',
                summary: 'Error',
                detail: err.message || 'Failed to delete patient'
              });
              this.loading.set(false);
            }
          });
      }
    });
  }
}

