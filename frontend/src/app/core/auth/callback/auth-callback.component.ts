import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';

@Component({
    selector: 'app-auth-callback',
    standalone: true,
    imports: [CommonModule],
    template: `
    <div class="flex items-center justify-center min-h-screen bg-gray-100">
      <div class="text-center">
        <div class="inline-block animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mb-4"></div>
        <p class="text-gray-700 text-lg">Completing login...</p>
      </div>
    </div>
  `
})
export class AuthCallbackComponent implements OnInit {
    private readonly route = inject(ActivatedRoute);
    private readonly router = inject(Router);
    private readonly authService = inject(AuthService);

    async ngOnInit() {
        try {
            // Get authorization code from URL
            const code = this.route.snapshot.queryParams['code'];
            const error = this.route.snapshot.queryParams['error'];

            if (error) {
                console.error('OAuth error:', error);
                this.router.navigate(['/login'], {
                    queryParams: { error: 'Authentication failed' }
                });
                return;
            }

            if (!code) {
                console.error('No authorization code received');
                this.router.navigate(['/login'], {
                    queryParams: { error: 'No authorization code' }
                });
                return;
            }

            // Exchange code for tokens
            const success = await this.authService.handleCallback(code);

            if (success) {
                // Redirect to dashboard or return URL
                const returnUrl = sessionStorage.getItem('returnUrl') || '/dashboard';
                sessionStorage.removeItem('returnUrl');
                this.router.navigate([returnUrl]);
            } else {
                this.router.navigate(['/login'], {
                    queryParams: { error: 'Failed to complete login' }
                });
            }
        } catch (error) {
            console.error('Error in auth callback:', error);
            this.router.navigate(['/login'], {
                queryParams: { error: 'An error occurred during login' }
            });
        }
    }
}
