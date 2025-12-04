import { Component, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { RippleModule } from 'primeng/ripple';
import { DepartmentsService } from '../services/departments.service';
import { DepartmentListModel } from '../models';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { catchError, of } from 'rxjs';

@Component({
  selector: 'app-departments-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, TableModule, ButtonModule, InputTextModule, RippleModule],
  templateUrl: './departments-list.html',
  styleUrl: './departments-list.css'
})
export class DepartmentsListComponent {
  private readonly service = inject(DepartmentsService);

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
          this.departments.set(response.data);
          this.loading.set(false);
        },
        error: () => this.loading.set(false)
      });
  }
}

