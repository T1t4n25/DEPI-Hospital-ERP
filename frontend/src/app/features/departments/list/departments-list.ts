import { Component, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { RippleModule } from 'primeng/ripple';
import { TooltipModule } from 'primeng/tooltip';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { MessageService, ConfirmationService } from 'primeng/api';
import { DepartmentsService } from '../services/departments.service';
import { DepartmentListModel } from '../models';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { catchError, of } from 'rxjs';
import { LoadingSpinnerComponent } from '../../../shared/components/loading-spinner/loading-spinner.component';

@Component({
  selector: 'app-departments-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    TableModule,
    ButtonModule,
    InputTextModule,
    RippleModule,
    TooltipModule,
    ToastModule,
    ConfirmDialogModule,
    LoadingSpinnerComponent
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './departments-list.html',
  styleUrl: './departments-list.css'
})
export class DepartmentsListComponent {
  private readonly service = inject(DepartmentsService);
  private readonly messageService = inject(MessageService);
  private readonly confirmationService = inject(ConfirmationService);

  readonly loading = signal<boolean>(false);
  readonly error = signal<string | null>(null);
  readonly departments = signal<DepartmentListModel[]>([]);
  readonly searchTerm = signal<string>('');

  readonly filteredDepartments = computed(() => {
    const items = this.departments();
    const term = this.searchTerm().toLowerCase();
    if (!term) return items;

    return items.filter(d =>
      d.departmentName.toLowerCase().includes(term) ||
      (d.managerName && d.managerName.toLowerCase().includes(term))
    );
  });

  constructor() {
    this.loadDepartments();
  }

  loadDepartments() {
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
          this.departments.set(response.data);
          this.loading.set(false);
        },
        error: () => this.loading.set(false)
      });
  }

  deleteDepartment(department: DepartmentListModel) {
    this.confirmationService.confirm({
      message: `Are you sure you want to delete ${department.departmentName}?`,
      header: 'Confirm Deletion',
      icon: 'pi pi-exclamation-triangle',
      acceptButtonStyleClass: 'p-button-danger',
      accept: () => {
        this.loading.set(true);
        this.service.delete(department.departmentID)
          .subscribe({
            next: () => {
              this.messageService.add({
                severity: 'success',
                summary: 'Success',
                detail: 'Department deleted successfully'
              });
              this.loadDepartments();
            },
            error: (err: Error) => {
              this.messageService.add({
                severity: 'error',
                summary: 'Error',
                detail: err.message || 'Failed to delete department'
              });
              this.loading.set(false);
            }
          });
      }
    });
  }
}
