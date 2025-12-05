import { Component, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TableModule } from 'primeng/table';
import { InputTextModule } from 'primeng/inputtext';
import { TagModule } from 'primeng/tag';
import { RippleModule } from 'primeng/ripple';
import { InventoryService } from '../../inventory/services/inventory.service';
import { InventoryListModel } from '../../inventory/models';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { catchError, of } from 'rxjs';
import { LoadingSpinnerComponent } from '../../../shared/components/loading-spinner/loading-spinner.component';

@Component({
  selector: 'app-pharmacy-inventory',
  standalone: true,
  imports: [CommonModule, FormsModule, TableModule, InputTextModule, TagModule, RippleModule, LoadingSpinnerComponent],
  templateUrl: './pharmacy-inventory.html',
  styleUrl: './pharmacy-inventory.css'
})
export class PharmacyInventoryComponent {
  private readonly service = inject(InventoryService);

  readonly loading = signal<boolean>(false);
  readonly error = signal<string | null>(null);
  readonly inventory = signal<InventoryListModel[]>([]);
  readonly searchTerm = signal<string>('');

  readonly filteredInventory = computed(() => {
    const items = this.inventory();
    const term = this.searchTerm().toLowerCase();
    if (!term) return items;

    return items.filter(item =>
      item.medicationName.toLowerCase().includes(term) ||
      item.barCode.toLowerCase().includes(term)
    );
  });

  constructor() {
    this.loadInventory();
  }

  loadInventory() {
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
          this.inventory.set(response.data);
          this.loading.set(false);
        },
        error: () => this.loading.set(false)
      });
  }

  getStatusSeverity(expiryDate: string): 'success' | 'warn' | 'danger' {
    const expiry = new Date(expiryDate);
    const today = new Date();
    const daysUntilExpiry = Math.floor((expiry.getTime() - today.getTime()) / (1000 * 60 * 60 * 24));

    if (daysUntilExpiry < 0) return 'danger';
    if (daysUntilExpiry < 90) return 'warn';
    return 'success';
  }

  getStatusLabel(expiryDate: string): string {
    const expiry = new Date(expiryDate);
    const today = new Date();
    const daysUntilExpiry = Math.floor((expiry.getTime() - today.getTime()) / (1000 * 60 * 60 * 24));

    if (daysUntilExpiry < 0) return 'Expired';
    if (daysUntilExpiry < 90) return 'Expiring Soon';
    return 'Good';
  }
}

