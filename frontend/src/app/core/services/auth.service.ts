import { Injectable, signal, computed, inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';
import { firstValueFrom } from 'rxjs';

export interface User {
  name: string;
  role: string;
  email: string;
  roles?: string[];
  sub?: string;
}

export interface KeycloakTokenResponse {
  access_token: string;
  refresh_token: string;
  expires_in: number;
  refresh_expires_in: number;
  token_type: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);

  private _token = signal<string | null>(null);
  private _refreshToken = signal<string | null>(null);
  private _currentUser = signal<User | null>(null);

  readonly isAuthenticated = computed(() => !!this._token());
  readonly currentUser = computed(() => this._currentUser());

  private readonly keycloakUrl = environment.keycloakUrl;
  private readonly realm = environment.keycloakRealm;
  private readonly clientId = environment.keycloakClientId;
  private readonly redirectUri = `${window.location.origin}/auth/callback`;

  constructor() {
    // Load tokens from localStorage on initialization
    const storedToken = localStorage.getItem('access_token');
    const storedRefreshToken = localStorage.getItem('refresh_token');

    if (storedToken) {
      this._token.set(storedToken);
      this._refreshToken.set(storedRefreshToken);
      this.loadUserFromToken(storedToken);
    }
  }

  /**
   * Initiates Keycloak login using Authorization Code Flow with PKCE
   */
  async initiateLogin(): Promise<void> {
    // Generate PKCE code verifier and challenge
    const codeVerifier = this.generateCodeVerifier();
    const codeChallenge = await this.generateCodeChallenge(codeVerifier);

    // Store code verifier for later use
    sessionStorage.setItem('pkce_code_verifier', codeVerifier);

    // Build authorization URL
    const authUrl = this.buildAuthorizationUrl(codeChallenge);

    // Redirect to Keycloak login
    window.location.href = authUrl;
  }

  /**
   * Handles the OAuth callback and exchanges code for tokens
   */
  async handleCallback(code: string): Promise<boolean> {
    try {
      const codeVerifier = sessionStorage.getItem('pkce_code_verifier');
      if (!codeVerifier) {
        throw new Error('Code verifier not found');
      }

      const tokenResponse = await this.exchangeCodeForToken(code, codeVerifier);

      // Store tokens
      this.setTokens(tokenResponse.access_token, tokenResponse.refresh_token);

      // Clear code verifier
      sessionStorage.removeItem('pkce_code_verifier');

      return true;
    } catch (error) {
      console.error('Error handling callback:', error);
      return false;
    }
  }

  /**
   * Exchange authorization code for access and refresh tokens
   */
  private async exchangeCodeForToken(code: string, codeVerifier: string): Promise<KeycloakTokenResponse> {
    const tokenUrl = `${this.keycloakUrl}/realms/${this.realm}/protocol/openid-connect/token`;

    const body = new URLSearchParams({
      grant_type: 'authorization_code',
      client_id: this.clientId,
      code: code,
      redirect_uri: this.redirectUri,
      code_verifier: codeVerifier
    });

    const headers = new HttpHeaders({
      'Content-Type': 'application/x-www-form-urlencoded'
    });

    return firstValueFrom(
      this.http.post<KeycloakTokenResponse>(tokenUrl, body.toString(), { headers })
    );
  }

  /**
   * Refresh the access token using refresh token
   */
  async refreshAccessToken(): Promise<boolean> {
    try {
      const refreshToken = this._refreshToken();
      if (!refreshToken) {
        return false;
      }

      const tokenUrl = `${this.keycloakUrl}/realms/${this.realm}/protocol/openid-connect/token`;

      const body = new URLSearchParams({
        grant_type: 'refresh_token',
        client_id: this.clientId,
        refresh_token: refreshToken
      });

      const headers = new HttpHeaders({
        'Content-Type': 'application/x-www-form-urlencoded'
      });

      const tokenResponse = await firstValueFrom(
        this.http.post<KeycloakTokenResponse>(tokenUrl, body.toString(), { headers })
      );

      this.setTokens(tokenResponse.access_token, tokenResponse.refresh_token);
      return true;
    } catch (error) {
      console.error('Error refreshing token:', error);
      this.logout();
      return false;
    }
  }

  /**
   * Build Keycloak authorization URL
   */
  private buildAuthorizationUrl(codeChallenge: string): string {
    const params = new URLSearchParams({
      client_id: this.clientId,
      redirect_uri: this.redirectUri,
      response_type: 'code',
      scope: 'openid profile email',
      code_challenge: codeChallenge,
      code_challenge_method: 'S256'
    });

    return `${this.keycloakUrl}/realms/${this.realm}/protocol/openid-connect/auth?${params.toString()}`;
  }

  /**
   * Generate PKCE code verifier
   */
  private generateCodeVerifier(): string {
    const array = new Uint8Array(32);
    crypto.getRandomValues(array);
    return this.base64UrlEncode(array);
  }

  /**
   * Generate PKCE code challenge from verifier
   */
  private async generateCodeChallenge(verifier: string): Promise<string> {
    const encoder = new TextEncoder();
    const data = encoder.encode(verifier);
    const hash = await crypto.subtle.digest('SHA-256', data);
    return this.base64UrlEncode(new Uint8Array(hash));
  }

  /**
   * Base64 URL encode
   */
  private base64UrlEncode(array: Uint8Array): string {
    const base64 = btoa(String.fromCharCode(...array));
    return base64
      .replace(/\+/g, '-')
      .replace(/\//g, '_')
      .replace(/=/g, '');
  }

  /**
   * Set tokens and load user info
   */
  private setTokens(accessToken: string, refreshToken: string): void {
    this._token.set(accessToken);
    this._refreshToken.set(refreshToken);

    localStorage.setItem('access_token', accessToken);
    localStorage.setItem('refresh_token', refreshToken);

    this.loadUserFromToken(accessToken);
  }

  /**
   * Load user information from JWT token
   */
  private loadUserFromToken(token: string): void {
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));

      this._currentUser.set({
        name: payload.name || payload.preferred_username || 'User',
        email: payload.email || '',
        role: payload.realm_access?.roles?.[0] || 'User',
        roles: payload.realm_access?.roles || [],
        sub: payload.sub
      });
    } catch (error) {
      console.error('Error decoding token:', error);
    }
  }

  /**
   * Get current access token
   */
  getToken(): string | null {
    return this._token();
  }

  /**
   * Logout user
   */
  logout(): void {
    // Clear local state
    this._token.set(null);
    this._refreshToken.set(null);
    this._currentUser.set(null);

    localStorage.removeItem('access_token');
    localStorage.removeItem('refresh_token');
    sessionStorage.removeItem('pkce_code_verifier');

    // Redirect to Keycloak logout
    const logoutUrl = `${this.keycloakUrl}/realms/${this.realm}/protocol/openid-connect/logout?redirect_uri=${encodeURIComponent(window.location.origin)}`;
    window.location.href = logoutUrl;
  }

  /**
   * Check if user has specific role
   */
  hasRole(role: string): boolean {
    const user = this._currentUser();
    return user?.roles?.includes(role) || false;
  }

  /**
   * Check if token is expired
   */
  isTokenExpired(): boolean {
    const token = this._token();
    if (!token) return true;

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      const exp = payload.exp * 1000; // Convert to milliseconds
      return Date.now() >= exp;
    } catch {
      return true;
    }
  }
}
