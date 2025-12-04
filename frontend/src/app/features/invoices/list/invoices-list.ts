import { Component, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { TagModule } from 'primeng/tag';
import { RippleModule } from 'primeng/ripple';
import { InvoicesService } from '../services/invoices.service';
import { InvoiceListModel } from '../models';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { catchError, of } from 'rxjs';

@Component({
  selector: 'app-invoices-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, TableModule, ButtonModule, InputTextModule, TagModule, RippleModule],
  templateUrl: './invoices-list.html',
  styleUrl: './invoices-list.css'
})
export class InvoicesListComponent {
  private readonly service = inject(InvoicesService);

  readonly loading = signal<boolean>(false);
  readonly error = signal<string | null>(null);
  readonly invoices = signal<InvoiceListModel[]>([]);
  readonly searchTerm = signal<string>('');

  readonly filteredInvoices = computed(() => {
    const items = this.invoices();
    const term = this.searchTerm().toLowerCase();
    if (!term) return items;

    return items.filter(inv =>
      inv.patientName.toLowerCase().includes(term) ||
      inv.invoiceTypeName.toLowerCase().includes(term)
    );
  });

  constructor() {
    this.loadInvoices();
  }

  loadInvoices() {
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
          this.invoices.set(response.data);
          this.loading.set(false);
        },
        error: () => this.loading.set(false)
      });
  }

  getStatusSeverity(status: string): 'success' | 'warn' {
    return status.toLowerCase().includes('paid') ? 'success' : 'warn';
  }
}

