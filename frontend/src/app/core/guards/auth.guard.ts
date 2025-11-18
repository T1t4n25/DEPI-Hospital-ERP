import { inject } from '@angular/core';
import { Router, CanActivateFn } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  // Placeholder - will be implemented in Phase 2
  // For Phase 1, always allow access
  return true;
  
  // Phase 2 implementation:
  // if (authService.isAuthenticated()) {
  //   return true;
  // }
  // router.navigate(['/login']);
  // return false;
};

