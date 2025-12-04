import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry, shareReplay } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { PaginatedResponse } from '../../../shared/interfaces/paginated-response.interface';
import { QueryParams } from '../../../shared/interfaces/query-params.interface';
import {
    EmployeeListModel,
    EmployeeDetailModel,
    CreateEmployeeModel,
    UpdateEmployeeModel
} from '../models';

@Injectable({ providedIn: 'root' })
export class EmployeesService {
    private readonly http = inject(HttpClient);
    private readonly apiUrl = `${environment.apiUrl}/api/employees`;

    // Cache for frequently accessed data
    private readonly cache$ = new Map<string, Observable<any>>();

    getAll(params?: QueryParams): Observable<PaginatedResponse<EmployeeListModel>> {
        const httpParams = this.buildHttpParams(params);
        const cacheKey = `all-${httpParams.toString()}`;

        if (this.cache$.has(cacheKey)) {
            return this.cache$.get(cacheKey)!;
        }

        const request$ = this.http.get<PaginatedResponse<EmployeeListModel>>(this.apiUrl, { params: httpParams })
            .pipe(
                retry(2),
                shareReplay(1),
                catchError(this.handleError.bind(this))
            );

        this.cache$.set(cacheKey, request$);
        return request$;
    }

    getById(id: number): Observable<EmployeeDetailModel> {
        const cacheKey = `id-${id}`;

        if (this.cache$.has(cacheKey)) {
            return this.cache$.get(cacheKey)!;
        }

        const request$ = this.http.get<EmployeeDetailModel>(`${this.apiUrl}/${id}`)
            .pipe(
                retry(2),
                shareReplay(1),
                catchError(this.handleError.bind(this))
            );

        this.cache$.set(cacheKey, request$);
        return request$;
    }

    create(data: CreateEmployeeModel): Observable<EmployeeDetailModel> {
        this.clearCache();
        return this.http.post<EmployeeDetailModel>(this.apiUrl, data)
            .pipe(
                catchError(this.handleError.bind(this))
            );
    }

    update(id: number, data: UpdateEmployeeModel): Observable<EmployeeDetailModel> {
        this.clearCache();
        return this.http.put<EmployeeDetailModel>(`${this.apiUrl}/${id}`, data)
            .pipe(
                catchError(this.handleError.bind(this))
            );
    }

    delete(id: number): Observable<void> {
        this.clearCache();
        return this.http.delete<void>(`${this.apiUrl}/${id}`)
            .pipe(
                catchError(this.handleError.bind(this))
            );
    }

    // Clear cache (call after create/update/delete)
    clearCache(): void {
        this.cache$.clear();
    }

    // Clear specific cache entry
    clearCacheEntry(key: string): void {
        this.cache$.delete(key);
    }

    private buildHttpParams(params?: QueryParams): HttpParams {
        let httpParams = new HttpParams();

        if (params) {
            Object.keys(params).forEach(key => {
                const value = params[key];
                if (value !== undefined && value !== null && value !== '') {
                    httpParams = httpParams.set(key, value.toString());
                }
            });
        }

        // Set default pagination if not provided
        if (!params?.pageNumber) {
            httpParams = httpParams.set('pageNumber', '1');
        }
        if (!params?.pageSize) {
            httpParams = httpParams.set('pageSize', '10');
        }

        return httpParams;
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

        console.error('EmployeesService Error:', errorMessage);
        return throwError(() => new Error(errorMessage));
    }
}
