import { Component, inject, signal, computed, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class LoginComponent {
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);

  readonly loading = signal<boolean>(false);
  readonly error = signal<string | null>(null);

  form: FormGroup = this.fb.group({
    username: ['', [Validators.required, Validators.minLength(3)]],
    password: ['', [Validators.required, Validators.minLength(6)]],
    rememberMe: [false]
  });

  readonly isFormValid = computed(() => this.form.valid);

  ngOnInit(): void {
    // Check if already authenticated
    if (this.authService.isAuthenticated()) {
      this.redirectAfterLogin();
    }

    // Check for return URL
    const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/dashboard';
    this._returnUrl = returnUrl;
  }

  private _returnUrl = '/dashboard';

  onSubmit(): void {
    if (this.form.valid) {
      this.loading.set(true);
      this.error.set(null);

      const { username, password, rememberMe } = this.form.value;

      this.authService.login({ username, password })
        .then((success) => {
          this.loading.set(false);
          if (success) {
            this.redirectAfterLogin();
          } else {
            this.error.set('Invalid username or password');
          }
        })
        .catch((err) => {
          this.loading.set(false);
          this.error.set(err.message || 'An error occurred during login');
        });
    } else {
      this.form.markAllAsTouched();
    }
  }

  private redirectAfterLogin(): void {
    const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/dashboard';
    this.router.navigate([returnUrl]);
  }

  get username() {
    return this.form.get('username');
  }

  get password() {
    return this.form.get('password');
  }
}

