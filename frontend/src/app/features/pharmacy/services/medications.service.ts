import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry, shareReplay } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { PaginatedResponse } from '../../../shared/interfaces/paginated-response.interface';
import { QueryParams } from '../../../shared/interfaces/query-params.interface';
import {
    MedicationListModel,
    MedicationDetailModel,
    CreateMedicationModel,
    UpdateMedicationModel
} from '../models';

@Injectable({ providedIn: 'root' })
export class MedicationsService {
    private readonly http = inject(HttpClient);
    private readonly apiUrl = `${environment.apiUrl}/api/medications`;

    // Cache for frequently accessed data
    private readonly cache$ = new Map<string, Observable<any>>();

    getAll(params?: QueryParams): Observable<PaginatedResponse<MedicationListModel>> {
        const httpParams = this.buildHttpParams(params);
        const cacheKey = `all-${httpParams.toString()}`;

        if (this.cache$.has(cacheKey)) {
            return this.cache$.get(cacheKey)!;
        }

        const request$ = this.http.get<PaginatedResponse<MedicationListModel>>(this.apiUrl, { params: httpParams })
            .pipe(
                retry(2),
                shareReplay(1),
                catchError(this.handleError.bind(this))
            );

        this.cache$.set(cacheKey, request$);
        return request$;
    }

    getById(id: number): Observable<MedicationDetailModel> {
        const cacheKey = `id-${id}`;

        if (this.cache$.has(cacheKey)) {
            return this.cache$.get(cacheKey)!;
        }

        const request$ = this.http.get<MedicationDetailModel>(`${this.apiUrl}/${id}`)
            .pipe(
                retry(2),
                shareReplay(1),
                catchError(this.handleError.bind(this))
            );

        this.cache$.set(cacheKey, request$);
        return request$;
    }

    create(data: CreateMedicationModel): Observable<MedicationDetailModel> {
        this.clearCache();
        return this.http.post<MedicationDetailModel>(this.apiUrl, data)
            .pipe(
                catchError(this.handleError.bind(this))
            );
    }

    update(id: number, data: UpdateMedicationModel): Observable<MedicationDetailModel> {
        this.clearCache();
        return this.http.put<MedicationDetailModel>(`${this.apiUrl}/${id}`, data)
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

        console.error('MedicationsService Error:', errorMessage);
        return throwError(() => new Error(errorMessage));
    }
}
