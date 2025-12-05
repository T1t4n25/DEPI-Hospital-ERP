import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry, shareReplay, map } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { PaginatedResponse } from '../../../shared/interfaces/paginated-response.interface';
import { QueryParams } from '../../../shared/interfaces/query-params.interface';
import {
  MedicalRecordListModel,
  MedicalRecordDetailModel,
  CreateMedicalRecordModel,
  UpdateMedicalRecordModel
} from '../models';

@Injectable({ providedIn: 'root' })
export class MedicalRecordsService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/api/medical-records`;
  private readonly cache$ = new Map<string, Observable<any>>();

  getAll(params?: QueryParams): Observable<PaginatedResponse<MedicalRecordListModel>> {
    const httpParams = this.buildHttpParams(params);
    const cacheKey = `all-${httpParams.toString()}`;

    if (this.cache$.has(cacheKey)) {
      return this.cache$.get(cacheKey)!;
    }

    const request$ = this.http.get<PaginatedResponse<MedicalRecordListModel>>(this.apiUrl, { params: httpParams })
      .pipe(retry(2), shareReplay(1), catchError(this.handleError.bind(this)));

    this.cache$.set(cacheKey, request$);
    return request$;
  }

  getById(id: number): Observable<MedicalRecordDetailModel> {
    const cacheKey = `id-${id}`;
    if (this.cache$.has(cacheKey)) {
      return this.cache$.get(cacheKey)!;
    }

    const request$ = this.http.get<MedicalRecordDetailModel>(`${this.apiUrl}/${id}`)
      .pipe(retry(2), shareReplay(1), catchError(this.handleError.bind(this)));

    this.cache$.set(cacheKey, request$);
    return request$;
  }

  create(data: CreateMedicalRecordModel): Observable<MedicalRecordDetailModel> {
    return this.http.post<MedicalRecordDetailModel>(this.apiUrl, data)
      .pipe(
        map(result => {
          this.clearCache();
          return result;
        }),
        catchError(this.handleError.bind(this))
      );
  }

  update(id: number, data: UpdateMedicalRecordModel): Observable<MedicalRecordDetailModel> {
    return this.http.put<MedicalRecordDetailModel>(`${this.apiUrl}/${id}`, data)
      .pipe(
        map(result => {
          this.clearCache();
          return result;
        }),
        catchError(this.handleError.bind(this))
      );
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`)
      .pipe(
        map(result => {
          this.clearCache();
          return result;
        }),
        catchError(this.handleError.bind(this))
      );
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

  private handleError(error: any): Observable<never> {
    const errorMessage = error.error?.message || error.message || 'An unknown error occurred';
    console.error('MedicalRecordsService Error:', errorMessage);
    return throwError(() => new Error(errorMessage));
  }
}
