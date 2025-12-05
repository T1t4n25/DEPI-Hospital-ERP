import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import {
  AdminDashboardDto,
  HrDashboardDto,
  AccountantDashboardDto,
  PharmacyDashboardDto
} from '../models/dashboard.models';

@Injectable({ providedIn: 'root' })
export class DashboardService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/api/dashboard`;

  getAdminDashboard(): Observable<AdminDashboardDto> {
    return this.http.get<AdminDashboardDto>(`${this.apiUrl}/admin`)
      .pipe(
        retry(2),
        catchError(this.handleError.bind(this))
      );
  }

  getHrDashboard(): Observable<HrDashboardDto> {
    return this.http.get<HrDashboardDto>(`${this.apiUrl}/hr`)
      .pipe(
        retry(2),
        catchError(this.handleError.bind(this))
      );
  }

  getAccountantDashboard(): Observable<AccountantDashboardDto> {
    return this.http.get<AccountantDashboardDto>(`${this.apiUrl}/accountant`)
      .pipe(
        retry(2),
        catchError(this.handleError.bind(this))
      );
  }

  getPharmacyDashboard(): Observable<PharmacyDashboardDto> {
    return this.http.get<PharmacyDashboardDto>(`${this.apiUrl}/pharmacy`)
      .pipe(
        retry(2),
        catchError(this.handleError.bind(this))
      );
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'An unknown error occurred';

    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Server-side error
      errorMessage = error.error?.message || error.message || `Error Code: ${error.status}`;
      if (error.error?.errors) {
        // Handle validation errors
        const validationErrors = Object.values(error.error.errors).flat();
        errorMessage = validationErrors.join(', ');
      }
    }

    console.error('DashboardService Error:', errorMessage);
    return throwError(() => new Error(errorMessage));
  }
}

