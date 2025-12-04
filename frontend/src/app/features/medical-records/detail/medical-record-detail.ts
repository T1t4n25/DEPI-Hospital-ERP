import { Component, inject, signal, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute } from '@angular/router';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { MedicalRecordsService } from '../services/medical-records.service';
import { MedicalRecordDetailModel } from '../models';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { catchError, of } from 'rxjs';
import { LoadingSpinnerComponent } from '../../../shared/components/loading-spinner/loading-spinner.component';

@Component({
  selector: 'app-medical-record-detail',
  standalone: true,
  imports: [CommonModule, RouterModule, CardModule, ButtonModule, LoadingSpinnerComponent],
  templateUrl: './medical-record-detail.html',
  styleUrl: './medical-record-detail.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MedicalRecordDetailComponent {
  private readonly service = inject(MedicalRecordsService);
  private readonly route = inject(ActivatedRoute);

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
}

