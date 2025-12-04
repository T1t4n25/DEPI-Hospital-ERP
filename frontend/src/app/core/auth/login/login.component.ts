import { Component, inject, signal, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class LoginComponent implements OnInit {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);

  readonly loading = signal<boolean>(false);
  readonly error = signal<string | null>(null);

  ngOnInit(): void {
    // Check if already authenticated
    if (this.authService.isAuthenticated()) {
      this.router.navigate(['/dashboard']);
      return;
    }

    // Check for error from callback
    const errorParam = this.route.snapshot.queryParams['error'];
    if (errorParam) {
      this.error.set(errorParam);
    }
  }

  loginWithKeycloak(): void {
    this.loading.set(true);
    this.error.set(null);

    // Store return URL if provided
    const returnUrl = this.route.snapshot.queryParams['returnUrl'];
    if (returnUrl) {
      sessionStorage.setItem('returnUrl', returnUrl);
    }

    // Initiate Keycloak login
    this.authService.initiateLogin();
  }
}

