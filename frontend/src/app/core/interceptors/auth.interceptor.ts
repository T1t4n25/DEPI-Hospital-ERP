import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  // Get token from auth service (to be implemented with Keycloak)
  const token = authService.getToken();

  // Clone request and add authorization header if token exists
  if (token) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      // Handle 401 Unauthorized
      if (error.status === 401) {
        authService.logout();
        router.navigate(['/login']);
      }

      // Handle 403 Forbidden
      if (error.status === 403) {
        // Could show access denied message
        console.error('Access denied');
      }

      return throwError(() => error);
    })
  );
};

