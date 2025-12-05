import { Component, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TableModule } from 'primeng/table';
import { InputTextModule } from 'primeng/inputtext';
import { TagModule } from 'primeng/tag';
import { EmployeesService } from '../../employees/services/employees.service';
import { EmployeeListModel } from '../../employees/models/employee-list.model';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { catchError, of } from 'rxjs';
import { LoadingSpinnerComponent } from '../../../shared/components/loading-spinner/loading-spinner.component';

@Component({
  selector: 'app-hr-employees',
  standalone: true,
  imports: [CommonModule, FormsModule, TableModule, InputTextModule, TagModule, LoadingSpinnerComponent],
  templateUrl: './hr-employees.html',
  styleUrl: './hr-employees.css'
})
export class HrEmployeesComponent {
  private readonly service = inject(EmployeesService);

  readonly loading = signal<boolean>(false);
  readonly error = signal<string | null>(null);
  readonly employees = signal<EmployeeListModel[]>([]);
  readonly searchTerm = signal<string>('');

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
    this.loadEmployees();
  }

  loadEmployees() {
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
          this.employees.set(response.data);
          this.loading.set(false);
        },
        error: () => this.loading.set(false)
      });
  }
}

