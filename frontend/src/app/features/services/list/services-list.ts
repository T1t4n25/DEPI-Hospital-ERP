import { Component, inject, signal, computed, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { RippleModule } from 'primeng/ripple';
import { TooltipModule } from 'primeng/tooltip';
import { ServicesService } from '../services/services.service';
import { ServiceListModel } from '../models';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { catchError, of } from 'rxjs';
import { LoadingSpinnerComponent } from '../../../shared/components/loading-spinner/loading-spinner.component';

@Component({
  selector: 'app-services-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, TableModule, ButtonModule, InputTextModule, RippleModule, TooltipModule, LoadingSpinnerComponent],
  templateUrl: './services-list.html',
  styleUrl: './services-list.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ServicesListComponent {
  private readonly service = inject(ServicesService);

  readonly loading = signal<boolean>(false);
  readonly error = signal<string | null>(null);
  readonly services = signal<ServiceListModel[]>([]);
  readonly searchTerm = signal<string>('');

  readonly filteredServices = computed(() => {
    const items = this.services();
    const term = this.searchTerm().toLowerCase();
    if (!term) return items;

    return items.filter(s =>
      s.serviceName.toLowerCase().includes(term) ||
      s.departmentName.toLowerCase().includes(term)
    );
  });

  constructor() {
    this.loadServices();
  }

  loadServices() {
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
          this.services.set(response.data);
          this.loading.set(false);
        },
        error: () => this.loading.set(false)
      });
  }
}

