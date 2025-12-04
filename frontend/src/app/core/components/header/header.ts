import { Component, signal, computed, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './header.html',
  styleUrl: './header.css'
})
export class HeaderComponent {
  private readonly authService = inject(AuthService);

  showUserMenu = signal(false);
  showNotifications = signal(false);

  notifications = [
    { id: 1, message: 'New appointment scheduled', time: '5 min ago', read: false },
    { id: 2, message: 'Patient check-in required', time: '15 min ago', read: false },
    { id: 3, message: 'Inventory low alert', time: '1 hour ago', read: true }
  ];

  // Computed properties for user data from Keycloak
  readonly currentUser = computed(() => this.authService.currentUser());
  readonly isAuthenticated = computed(() => this.authService.isAuthenticated());

  readonly userAvatar = computed(() => {
    const user = this.currentUser();
    if (!user) return '';
    const name = user.name || 'User';
    return `https://ui-avatars.com/api/?name=${encodeURIComponent(name)}&background=0ea5e9&color=fff&size=128`;
  });

  toggleUserMenu() {
    this.showUserMenu.set(!this.showUserMenu());
  }

  toggleNotifications() {
    this.showNotifications.set(!this.showNotifications());
  }

  get unreadCount() {
    return this.notifications.filter(n => !n.read).length;
  }

  logout(event: Event) {
    event.preventDefault();
    this.authService.logout();
  }
}

