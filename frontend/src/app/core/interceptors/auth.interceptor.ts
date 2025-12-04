import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { catchError, switchMap, throwError, from } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  // Skip token attachment for Keycloak token endpoint
  if (req.url.includes('/protocol/openid-connect/token') ||
    req.url.includes('/auth/') ||
    req.url.includes('keycloak')) {
    return next(req);
  }

  const token = authService.getToken();

  // If no token, proceed without authorization header
  if (!token) {
    return next(req);
  }

  // Check if token is expired
  if (authService.isTokenExpired()) {
    // Try to refresh the token
    return from(authService.refreshAccessToken()).pipe(
      switchMap(success => {
        if (success) {
          const newToken = authService.getToken();
          const clonedReq = req.clone({
            setHeaders: {
              Authorization: `Bearer ${newToken}`
            }
          });
          return next(clonedReq);
        } else {
          // Refresh failed, redirect to login
          router.navigate(['/login']);
          return throwError(() => new Error('Token expired and refresh failed'));
        }
      })
    );
  }

  // Clone request and add authorization header
  const clonedReq = req.clone({
    setHeaders: {
      Authorization: `Bearer ${token}`
    }
  });

  return next(clonedReq).pipe(
    catchError((error) => {
      // Handle 401 Unauthorized
      if (error.status === 401) {
        // Try to refresh token
        return from(authService.refreshAccessToken()).pipe(
          switchMap(success => {
            if (success) {
              const newToken = authService.getToken();
              const retryReq = req.clone({
                setHeaders: {
                  Authorization: `Bearer ${newToken}`
                }
              });
              return next(retryReq);
            } else {
              router.navigate(['/login']);
              return throwError(() => error);
            }
          })
        );
      }
      return throwError(() => error);
    })
  );
};
