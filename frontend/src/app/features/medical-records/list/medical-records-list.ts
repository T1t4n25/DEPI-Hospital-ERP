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
import { MedicalRecordsService } from '../services/medical-records.service';
import { MedicalRecordListModel } from '../models/medical-record-list.model';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { catchError, of } from 'rxjs';
import { LoadingSpinnerComponent } from '../../../shared/components/loading-spinner/loading-spinner.component';

@Component({
  selector: 'app-medical-records-list',
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
  templateUrl: './medical-records-list.html',
  styleUrl: './medical-records-list.css'
})
export class MedicalRecordsListComponent {
  private readonly service = inject(MedicalRecordsService);
  private readonly messageService = inject(MessageService);
  private readonly confirmationService = inject(ConfirmationService);

  readonly loading = signal<boolean>(false);
  readonly error = signal<string | null>(null);
  readonly records = signal<MedicalRecordListModel[]>([]);
  readonly searchTerm = signal<string>('');

  readonly filteredRecords = computed(() => {
    const items = this.records();
    const term = this.searchTerm().toLowerCase();
    if (!term) return items;

    return items.filter(r =>
      r.patientName.toLowerCase().includes(term) ||
      r.doctorName.toLowerCase().includes(term) ||
      r.diagnosis.toLowerCase().includes(term)
    );
  });

  constructor() {
    this.loadRecords();
  }

  loadRecords() {
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
          this.records.set(response.data);
          this.loading.set(false);
        },
        error: () => this.loading.set(false)
      });
  }

  deleteRecord(record: MedicalRecordListModel) {
    this.confirmationService.confirm({
      message: `Are you sure you want to delete the medical record for ${record.patientName}?`,
      header: 'Confirm Deletion',
      icon: 'pi pi-exclamation-triangle',
      acceptButtonStyleClass: 'p-button-danger',
      accept: () => {
        this.loading.set(true);
        this.service.delete(record.recordID)
          .subscribe({
            next: () => {
              this.messageService.add({
                severity: 'success',
                summary: 'Success',
                detail: 'Medical record deleted successfully'
              });
              this.loadRecords();
            },
            error: (err: Error) => {
              this.messageService.add({
                severity: 'error',
                summary: 'Error',
                detail: err.message || 'Failed to delete medical record'
              });
              this.loading.set(false);
            }
          });
      }
    });
  }
}
