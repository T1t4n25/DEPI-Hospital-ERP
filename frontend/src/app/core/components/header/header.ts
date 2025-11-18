import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './header.html',
  styleUrl: './header.css'
})
export class HeaderComponent {
  showUserMenu = signal(false);
  showNotifications = signal(false);

  notifications = [
    { id: 1, message: 'New appointment scheduled', time: '5 min ago', read: false },
    { id: 2, message: 'Patient check-in required', time: '15 min ago', read: false },
    { id: 3, message: 'Inventory low alert', time: '1 hour ago', read: true }
  ];

  user = {
    name: 'Dr. John Doe',
    role: 'Admin',
    avatar: 'https://ui-avatars.com/api/?name=John+Doe&background=0ea5e9&color=fff'
  };

  toggleUserMenu() {
    this.showUserMenu.set(!this.showUserMenu());
  }

  toggleNotifications() {
    this.showNotifications.set(!this.showNotifications());
  }

  get unreadCount() {
    return this.notifications.filter(n => !n.read).length;
  }
}

