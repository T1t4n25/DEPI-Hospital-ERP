import { Component, inject, signal, computed, ChangeDetectionStrategy, DestroyRef } from '@angular/core';
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
import { EmployeesService } from '../services/employees.service';
import { EmployeeListModel } from '../models/employee-list.model';
import { PaginatedResponse } from '../../../shared/interfaces/paginated-response.interface';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { catchError, of } from 'rxjs';
import { LoadingSpinnerComponent } from '../../../shared/components/loading-spinner/loading-spinner.component';

@Component({
  selector: 'app-employees-list',
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
  templateUrl: './employees-list.html',
  styleUrl: './employees-list.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [ConfirmationService, MessageService]
})
export class EmployeesListComponent {
  private readonly service = inject(EmployeesService);
  private readonly confirmationService = inject(ConfirmationService);
  private readonly messageService = inject(MessageService);
  private readonly destroyRef = inject(DestroyRef);

  readonly loading = signal<boolean>(false);
  readonly error = signal<string | null>(null);
  readonly employees = signal<EmployeeListModel[]>([]);
  readonly searchTerm = signal<string>('');
  readonly totalRecords = signal<number>(0);
  readonly currentPage = signal<number>(0);
  readonly pageSize = signal<number>(10);

  readonly filteredEmployees = computed(() => {
    const items = this.employees();
    const term = this.searchTerm().toLowerCase();
    if (!term) return items;

    return items.filter(e =>
      e.firstName.toLowerCase().includes(term) ||
      e.lastName.toLowerCase().includes(term) ||
      e.roleName.toLowerCase().includes(term) ||
      e.departmentName.toLowerCase().includes(term)
    );
  });

  constructor() {
    this.loadEmployees(0);
  }

  loadEmployees(page: number = 0) {
    this.loading.set(true);
    this.error.set(null);

    this.service.getAll({
      pageNumber: page + 1, // PrimeNG uses 0-based, backend uses 1-based
      pageSize: this.pageSize(),
      searchTerm: this.searchTerm() || undefined
    })
      .pipe(
        takeUntilDestroyed(this.destroyRef),
        catchError((err: Error) => {
          this.error.set(err.message);
          return of({ data: [], pageNumber: 1, pageSize: 10, totalCount: 0, totalPages: 0, hasPreviousPage: false, hasNextPage: false });
        })
      )
      .subscribe({
        next: (response: PaginatedResponse<EmployeeListModel>) => {
          this.employees.set(response.data);
          this.totalRecords.set(response.totalCount);
          this.currentPage.set(response.pageNumber - 1); // Convert to 0-based
          this.loading.set(false);
        },
        error: () => this.loading.set(false)
      });
  }

  onPageChange(event: any) {
    this.loadEmployees(event.page);
  }

  onSearchChange() {
    // Reset to first page when searching
    this.loadEmployees(0);
  }

  deleteEmployee(employee: EmployeeListModel) {
    this.confirmationService.confirm({
      message: `Are you sure you want to delete ${employee.firstName} ${employee.lastName}?`,
      header: 'Confirm Deletion',
      icon: 'pi pi-exclamation-triangle',
      acceptButtonStyleClass: 'p-button-danger',
      accept: () => {
        this.loading.set(true);
        this.service.delete(employee.employeeID)
          .pipe(
            takeUntilDestroyed(this.destroyRef),
            catchError((err: Error) => {
              this.messageService.add({
                severity: 'error',
                summary: 'Error',
                detail: err.message || 'Failed to delete employee'
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
                detail: 'Employee deleted successfully'
              });
              this.loadEmployees(this.currentPage());
            }
          });
      }
    });
  }
}

