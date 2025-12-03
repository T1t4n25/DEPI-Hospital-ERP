import { Injectable, signal, computed } from '@angular/core';
import { environment } from '../../../environments/environment';

export interface User {
  name: string;
  role: string;
  email: string;
  roles?: string[];
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private _token = signal<string | null>(null);
  private _currentUser = signal<User | null>(null);

  readonly isAuthenticated = computed(() => !!this._token());
  readonly currentUser = computed(() => this._currentUser());

  constructor() {
    // Load token from localStorage on initialization
    const storedToken = localStorage.getItem('auth_token');
    if (storedToken) {
      this._token.set(storedToken);
      // Decode token to get user info (simplified - in production, use proper JWT decoding)
      this.loadUserFromToken(storedToken);
    }
  }

  getToken(): string | null {
    return this._token();
  }

  setToken(token: string): void {
    this._token.set(token);
    localStorage.setItem('auth_token', token);
    this.loadUserFromToken(token);
  }

  private loadUserFromToken(token: string): void {
    try {
      // Decode JWT token (base64 decode the payload)
      const payload = JSON.parse(atob(token.split('.')[1]));
      
      // Extract user information from token claims
      this._currentUser.set({
        name: payload.name || payload.preferred_username || 'User',
        email: payload.email || '',
        role: payload.realm_access?.roles?.[0] || 'User',
        roles: payload.realm_access?.roles || []
      });
    } catch (error) {
      console.error('Error decoding token:', error);
      // Fallback user for development
      this._currentUser.set({
        name: 'User',
        email: '',
        role: 'User',
        roles: []
      });
    }
  }

  async login(credentials: { username: string; password: string }): Promise<boolean> {
    try {
      // Call Keycloak token endpoint directly
      const tokenUrl = `${environment.keycloakUrl}/realms/${environment.keycloakRealm}/protocol/openid-connect/token`;
      
      const params = new URLSearchParams({
        grant_type: 'password',
        client_id: environment.keycloakClientId,
        username: credentials.username,
        password: credentials.password,
        scope: 'openid profile email'
      });

      const response = await fetch(tokenUrl, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/x-www-form-urlencoded'
        },
        body: params.toString()
      });

      if (!response.ok) {
        const errorData = await response.json().catch(() => ({}));
        throw new Error(errorData.error_description || errorData.error || 'Invalid username or password');
      }

      const tokenData = await response.json();
      const accessToken = tokenData.access_token;

      if (accessToken) {
        this.setToken(accessToken);
        return true;
      }

      return false;
    } catch (error: any) {
      console.error('Login error:', error);
      // For development/fallback, allow login with mock token if Keycloak is unavailable
      if (error.message?.includes('fetch') || error.message?.includes('Failed to fetch')) {
        console.warn('Keycloak unavailable, using mock token for development');
        const mockToken = this.generateMockToken(credentials.username);
        this.setToken(mockToken);
        return true;
      }
      throw error;
    }
  }

  private generateMockToken(username: string): string {
    // Generate a mock JWT token for development when Keycloak is unavailable
    // WARNING: This is only for development/testing purposes
    const header = btoa(JSON.stringify({ alg: 'HS256', typ: 'JWT' }));
    const payload = btoa(JSON.stringify({
      sub: username,
      name: username,
      email: `${username}@hospital.com`,
      preferred_username: username,
      realm_access: {
        roles: ['Admin'] // Default role for development
      },
      exp: Math.floor(Date.now() / 1000) + 3600 // 1 hour expiry
    }));
    const signature = 'mock_signature';
    return `${header}.${payload}.${signature}`;
  }

  logout(): void {
    this._token.set(null);
    this._currentUser.set(null);
    localStorage.removeItem('auth_token');
  }

  hasRole(role: string): boolean {
    const user = this._currentUser();
    if (!user || !user.roles) return false;
    return user.roles.includes(role);
  }

  hasAnyRole(roles: string[]): boolean {
    const user = this._currentUser();
    if (!user || !user.roles) return false;
    return roles.some(role => user.roles!.includes(role));
  }
}

