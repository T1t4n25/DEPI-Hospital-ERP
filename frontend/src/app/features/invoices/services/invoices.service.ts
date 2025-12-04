import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry, shareReplay } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { PaginatedResponse } from '../../../shared/interfaces/paginated-response.interface';
import { QueryParams } from '../../../shared/interfaces/query-params.interface';
import {
    InvoiceListModel,
    InvoiceDetailModel,
    CreateInvoiceModel,
    UpdateInvoiceModel
} from '../models';

@Injectable({ providedIn: 'root' })
export class InvoicesService {
    private readonly http = inject(HttpClient);
    private readonly apiUrl = `${environment.apiUrl}/api/invoices`;
    private readonly cache$ = new Map<string, Observable<any>>();

    getAll(params?: QueryParams): Observable<PaginatedResponse<InvoiceListModel>> {
        const httpParams = this.buildHttpParams(params);
        const cacheKey = `all-${httpParams.toString()}`;

        if (this.cache$.has(cacheKey)) {
            return this.cache$.get(cacheKey)!;
        }

        const request$ = this.http.get<PaginatedResponse<InvoiceListModel>>(this.apiUrl, { params: httpParams })
            .pipe(retry(2), shareReplay(1), catchError(this.handleError.bind(this)));

        this.cache$.set(cacheKey, request$);
        return request$;
    }

    getById(id: number): Observable<InvoiceDetailModel> {
        const cacheKey = `id-${id}`;
        if (this.cache$.has(cacheKey)) {
            return this.cache$.get(cacheKey)!;
        }

        const request$ = this.http.get<InvoiceDetailModel>(`${this.apiUrl}/${id}`)
            .pipe(retry(2), shareReplay(1), catchError(this.handleError.bind(this)));

        this.cache$.set(cacheKey, request$);
        return request$;
    }

    create(data: CreateInvoiceModel): Observable<InvoiceDetailModel> {
        this.clearCache();
        return this.http.post<InvoiceDetailModel>(this.apiUrl, data)
            .pipe(catchError(this.handleError.bind(this)));
    }

    updatePaymentStatus(id: number, data: UpdateInvoiceModel): Observable<InvoiceDetailModel> {
        this.clearCache();
        return this.http.put<InvoiceDetailModel>(`${this.apiUrl}/${id}/payment-status`, data)
            .pipe(catchError(this.handleError.bind(this)));
    }

    clearCache(): void {
        this.cache$.clear();
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
        if (!params?.pageNumber) httpParams = httpParams.set('pageNumber', '1');
        if (!params?.pageSize) httpParams = httpParams.set('pageSize', '10');
        return httpParams;
    }

    private handleError(error: HttpErrorResponse): Observable<never> {
        const errorMessage = error.error?.message || error.message || 'An unknown error occurred';
        console.error('InvoicesService Error:', errorMessage);
        return throwError(() => new Error(errorMessage));
    }
}
