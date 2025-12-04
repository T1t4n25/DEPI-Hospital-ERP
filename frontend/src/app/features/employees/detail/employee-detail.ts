import { Component, inject, signal, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute } from '@angular/router';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { EmployeesService } from '../services/employees.service';
import { EmployeeDetailModel } from '../models';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { catchError, of } from 'rxjs';
import { LoadingSpinnerComponent } from '../../../shared/components/loading-spinner/loading-spinner.component';

@Component({
  selector: 'app-employee-detail',
  standalone: true,
  imports: [CommonModule, RouterModule, CardModule, ButtonModule, TagModule, LoadingSpinnerComponent],
  templateUrl: './employee-detail.html',
  styleUrl: './employee-detail.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class EmployeeDetailComponent {
  private readonly service = inject(EmployeesService);
  private readonly route = inject(ActivatedRoute);

  readonly loading = signal<boolean>(false);
  readonly error = signal<string | null>(null);
  readonly employee = signal<EmployeeDetailModel | null>(null);

  constructor() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadEmployee(+id);
    }
  }

  loadEmployee(id: number) {
    this.loading.set(true);
    this.error.set(null);

    this.service.getById(id)
      .pipe(
        takeUntilDestroyed(),
        catchError((err: Error) => {
          this.error.set(err.message);
          return of(null);
        })
      )
      .subscribe({
        next: (data) => {
          this.employee.set(data);
          this.loading.set(false);
        },
        error: () => this.loading.set(false)
      });
  }
}

