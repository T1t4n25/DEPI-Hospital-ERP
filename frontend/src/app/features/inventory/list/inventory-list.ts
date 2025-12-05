import { Component, inject, signal, computed, DestroyRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { TagModule } from 'primeng/tag';
import { RippleModule } from 'primeng/ripple';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ToastModule } from 'primeng/toast';
import { ConfirmationService, MessageService } from 'primeng/api';
import { InventoryService } from '../services/inventory.service';
import { InventoryListModel } from '../models';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { catchError, of } from 'rxjs';

@Component({
  selector: 'app-inventory-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, TableModule, ButtonModule, InputTextModule, TagModule, RippleModule, ConfirmDialogModule, ToastModule],
  templateUrl: './inventory-list.html',
  styleUrl: './inventory-list.css',
  providers: [ConfirmationService, MessageService]
})
export class InventoryListComponent {
  private readonly service = inject(InventoryService);
  private readonly confirmationService = inject(ConfirmationService);
  private readonly messageService = inject(MessageService);
  private readonly destroyRef = inject(DestroyRef);

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
        takeUntilDestroyed(this.destroyRef),
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

  deleteInventoryItem(item: InventoryListModel) {
    this.confirmationService.confirm({
      message: `Are you sure you want to delete ${item.medicationName}?`,
      header: 'Confirm Deletion',
      icon: 'pi pi-exclamation-triangle',
      acceptButtonStyleClass: 'p-button-danger',
      accept: () => {
        this.loading.set(true);
        this.service.delete(item.medicationID)
          .pipe(
            takeUntilDestroyed(this.destroyRef),
            catchError((err: Error) => {
              this.messageService.add({
                severity: 'error',
                summary: 'Error',
                detail: err.message || 'Failed to delete inventory item'
              });
              this.loading.set(false);
              return of(null);
            })
          )
          .subscribe({
            next: () => {
              this.messageService.add({
                severity: 'success',
                summary: 'Success',
                detail: 'Inventory item deleted successfully'
              });
              this.loadInventory();
            }
          });
      }
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
