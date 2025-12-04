import { Component, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { TagModule } from 'primeng/tag';
import { RippleModule } from 'primeng/ripple';
import { MedicalRecordsService } from '../services/medical-records.service';
import { MedicalRecordListModel } from '../models/medical-record-list.model';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { catchError, of } from 'rxjs';

@Component({
  selector: 'app-medical-records-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, TableModule, ButtonModule, InputTextModule, TagModule, RippleModule],
  templateUrl: './medical-records-list.html',
  styleUrl: './medical-records-list.css'
})
export class MedicalRecordsListComponent {
  private readonly service = inject(MedicalRecordsService);

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

    this.service.getAll()
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
}

