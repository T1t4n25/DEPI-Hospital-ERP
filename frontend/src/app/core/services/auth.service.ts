import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private _isAuthenticated = signal(false);
  private _currentUser = signal<any>(null);

  isAuthenticated = this._isAuthenticated.asReadonly();
  currentUser = this._currentUser.asReadonly();

  constructor() {
    // Placeholder - will be implemented in Phase 2
    this._isAuthenticated.set(true);
    this._currentUser.set({
      name: 'Dr. John Doe',
      role: 'Admin',
      email: 'john.doe@hospital.com'
    });
  }

  login(credentials: any): Promise<boolean> {
    // Placeholder - will be implemented in Phase 2
    return Promise.resolve(true);
  }

  logout(): void {
    // Placeholder - will be implemented in Phase 2
    this._isAuthenticated.set(false);
    this._currentUser.set(null);
  }
}

