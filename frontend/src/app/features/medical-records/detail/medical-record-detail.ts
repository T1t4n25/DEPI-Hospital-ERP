import { Component, inject, signal, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { MessageService, ConfirmationService } from 'primeng/api';
import { MedicalRecordsService } from '../services/medical-records.service';
import { MedicalRecordDetailModel } from '../models';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { catchError, of } from 'rxjs';
import { LoadingSpinnerComponent } from '../../../shared/components/loading-spinner/loading-spinner.component';

@Component({
  selector: 'app-medical-record-detail',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    CardModule,
    ButtonModule,
    ToastModule,
    ConfirmDialogModule,
    LoadingSpinnerComponent
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './medical-record-detail.html',
  styleUrl: './medical-record-detail.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MedicalRecordDetailComponent {
  private readonly service = inject(MedicalRecordsService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly messageService = inject(MessageService);
  private readonly confirmationService = inject(ConfirmationService);

  readonly loading = signal<boolean>(false);
  readonly error = signal<string | null>(null);
  readonly record = signal<MedicalRecordDetailModel | null>(null);

  constructor() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadRecord(+id);
    }
  }

  loadRecord(id: number) {
    this.loading.set(true);
    this.error.set(null);

    this.service.getById(id)
      .pipe(
        takeUntilDestroyed(),
        catchError((err: Error) => {
          this.error.set(err.message);
          return of(null);
        })
      )
      .subscribe({
        next: (data) => {
          this.record.set(data);
          this.loading.set(false);
        },
        error: () => this.loading.set(false)
      });
  }

  deleteRecord() {
    const currentRecord = this.record();
    if (!currentRecord) return;

    this.confirmationService.confirm({
      message: `Are you sure you want to delete this medical record?`,
      header: 'Confirm Deletion',
      icon: 'pi pi-exclamation-triangle',
      acceptButtonStyleClass: 'p-button-danger',
      accept: () => {
        this.loading.set(true);
        this.service.delete(currentRecord.recordID)
          .subscribe({
            next: () => {
              this.messageService.add({
                severity: 'success',
                summary: 'Success',
                detail: 'Medical record deleted successfully'
              });
              setTimeout(() => {
                this.router.navigate(['/medical-records']);
              }, 1000);
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
