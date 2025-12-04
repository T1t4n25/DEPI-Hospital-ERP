import { Component, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { TagModule } from 'primeng/tag';
import { RippleModule } from 'primeng/ripple';
import { EmployeesService } from '../services/employees.service';
import { EmployeeListModel } from '../models/employee-list.model';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { catchError, of } from 'rxjs';

@Component({
  selector: 'app-employees-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, TableModule, ButtonModule, InputTextModule, TagModule, RippleModule],
  templateUrl: './employees-list.html',
  styleUrl: './employees-list.css'
})
export class EmployeesListComponent {
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
          this.employees.set(response.data);
          this.loading.set(false);
        },
        error: () => this.loading.set(false)
      });
  }
}

