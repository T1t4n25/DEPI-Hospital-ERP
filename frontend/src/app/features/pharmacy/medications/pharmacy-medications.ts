import { Component, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TableModule } from 'primeng/table';
import { InputTextModule } from 'primeng/inputtext';
import { TagModule } from 'primeng/tag';
import { RippleModule } from 'primeng/ripple';
import { MedicationsService } from '../services/medications.service';
import { MedicationListModel } from '../models';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { catchError, of } from 'rxjs';

@Component({
  selector: 'app-pharmacy-medications',
  standalone: true,
  imports: [CommonModule, FormsModule, TableModule, InputTextModule, TagModule, RippleModule],
  templateUrl: './pharmacy-medications.html',
  styleUrl: './pharmacy-medications.css'
})
export class PharmacyMedicationsComponent {
  private readonly service = inject(MedicationsService);

  readonly loading = signal<boolean>(false);
  readonly error = signal<string | null>(null);
  readonly medications = signal<MedicationListModel[]>([]);
  readonly searchTerm = signal<string>('');

  readonly filteredMedications = computed(() => {
    const items = this.medications();
    const term = this.searchTerm().toLowerCase();
    if (!term) return items;

    return items.filter(med =>
      med.name.toLowerCase().includes(term) ||
      med.barCode.toLowerCase().includes(term)
    );
  });

  constructor() {
    this.loadMedications();
  }

  loadMedications() {
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
          this.medications.set(response.data);
          this.loading.set(false);
        },
        error: () => this.loading.set(false)
      });
  }

  getStockStatus(quantity?: number): string {
    if (!quantity || quantity === 0) return 'Out of Stock';
    if (quantity < 50) return 'Low Stock';
    return 'In Stock';
  }

  getStockSeverity(quantity?: number): 'success' | 'warn' | 'danger' {
    if (!quantity || quantity === 0) return 'danger';
    if (quantity < 50) return 'warn';
    return 'success';
  }
}

