import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      let errorMessage = 'An unknown error occurred';

      if (error.error instanceof ErrorEvent) {
        // Client-side error
        errorMessage = `Error: ${error.error.message}`;
      } else {
        // Server-side error
        errorMessage = error.error?.message || error.message || `Error Code: ${error.status}`;
      }

      // Log error for debugging (only in development)
      if (!error.url?.includes('/api/')) {
        console.error('HTTP Error:', {
          url: error.url,
          status: error.status,
          message: errorMessage,
          error: error.error
        });
      }

      return throwError(() => new Error(errorMessage));
    })
  );
};

