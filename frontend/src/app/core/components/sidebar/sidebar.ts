import { Component, signal, computed, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';

interface MenuItem {
  label: string;
  icon: string;
  route: string;
  roles?: string[];
  children?: MenuItem[];
}

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.css'
})
export class SidebarComponent {
  private authService = inject(AuthService);

  collapsed = signal(false);
  activeMenu = signal<string | null>(null);

  private readonly allMenuItems: MenuItem[] = [
    {
      label: 'Dashboard',
      icon: 'pi pi-home',
      route: '/dashboard'
    },
    {
      label: 'Patients',
      icon: 'pi pi-users',
      route: '/patients',
      roles: ['Admin', 'Doctor', 'Receptionist'],
      children: [
        { label: 'All Patients', icon: 'pi pi-list', route: '/patients', roles: ['Admin', 'Doctor', 'Receptionist'] },
        { label: 'Add Patient', icon: 'pi pi-plus', route: '/patients/new', roles: ['Admin', 'Receptionist'] }
      ]
    },
    {
      label: 'Appointments',
      icon: 'pi pi-calendar',
      route: '/appointments',
      roles: ['Admin', 'Doctor', 'Receptionist'],
      children: [
        { label: 'All Appointments', icon: 'pi pi-list', route: '/appointments', roles: ['Admin', 'Doctor', 'Receptionist'] },
        { label: 'New Appointment', icon: 'pi pi-plus', route: '/appointments/new', roles: ['Admin', 'Receptionist'] }
      ]
    },
    {
      label: 'Medical Records',
      icon: 'pi pi-file-medical',
      route: '/medical-records',
      roles: ['Admin', 'Doctor'],
      children: [
        { label: 'All Records', icon: 'pi pi-list', route: '/medical-records', roles: ['Admin', 'Doctor'] },
        { label: 'New Record', icon: 'pi pi-plus', route: '/medical-records/new', roles: ['Admin', 'Doctor'] }
      ]
    },
    {
      label: 'Employees',
      icon: 'pi pi-id-card',
      route: '/employees',
      roles: ['Admin'],
      children: [
        { label: 'All Employees', icon: 'pi pi-list', route: '/employees', roles: ['Admin'] },
        { label: 'Add Employee', icon: 'pi pi-plus', route: '/employees/new', roles: ['Admin'] }
      ]
    },
    {
      label: 'Departments',
      icon: 'pi pi-building',
      route: '/departments',
      roles: ['Admin'],
      children: [
        { label: 'All Departments', icon: 'pi pi-list', route: '/departments', roles: ['Admin'] },
        { label: 'Add Department', icon: 'pi pi-plus', route: '/departments/new', roles: ['Admin'] }
      ]
    },
    {
      label: 'HR',
      icon: 'pi pi-briefcase',
      route: '/hr',
      roles: ['Admin'],
      children: [
        { label: 'HR Dashboard', icon: 'pi pi-chart-bar', route: '/hr/dashboard', roles: ['Admin'] },
        { label: 'Employees', icon: 'pi pi-users', route: '/hr/employees', roles: ['Admin'] },
        { label: 'Attendance', icon: 'pi pi-clock', route: '/hr/attendance', roles: ['Admin'] },
        { label: 'Payroll', icon: 'pi pi-dollar', route: '/hr/payroll', roles: ['Admin'] }
      ]
    },
    {
      label: 'Pharmacy',
      icon: 'pi pi-shopping-cart',
      route: '/pharmacy',
      roles: ['Admin', 'Pharmacist'],
      children: [
        { label: 'Pharmacy Dashboard', icon: 'pi pi-chart-bar', route: '/pharmacy/dashboard', roles: ['Admin', 'Pharmacist'] },
        { label: 'Medications', icon: 'pi pi-box', route: '/pharmacy/medications', roles: ['Admin', 'Pharmacist'] },
        { label: 'Inventory', icon: 'pi pi-warehouse', route: '/pharmacy/inventory', roles: ['Admin', 'Pharmacist'] },
        { label: 'Prescriptions', icon: 'pi pi-file', route: '/pharmacy/prescriptions', roles: ['Admin', 'Pharmacist'] }
      ]
    },
    {
      label: 'Services',
      icon: 'pi pi-cog',
      route: '/services',
      roles: ['Admin'],
      children: [
        { label: 'All Services', icon: 'pi pi-list', route: '/services', roles: ['Admin'] },
        { label: 'Add Service', icon: 'pi pi-plus', route: '/services/new', roles: ['Admin'] }
      ]
    },
    {
      label: 'Invoices',
      icon: 'pi pi-file-edit',
      route: '/invoices',
      roles: ['Admin', 'Accountant', 'Receptionist'],
      children: [
        { label: 'All Invoices', icon: 'pi pi-list', route: '/invoices', roles: ['Admin', 'Accountant', 'Receptionist'] },
        { label: 'New Invoice', icon: 'pi pi-plus', route: '/invoices/new', roles: ['Admin', 'Receptionist'] }
      ]
    },
    {
      label: 'Inventory',
      icon: 'pi pi-warehouse',
      route: '/inventory',
      roles: ['Admin', 'Pharmacist'],
      children: [
        { label: 'All Inventory', icon: 'pi pi-list', route: '/inventory', roles: ['Admin', 'Pharmacist'] },
        { label: 'Add Item', icon: 'pi pi-plus', route: '/inventory/new', roles: ['Admin', 'Pharmacist'] }
      ]
    }
  ];

  menuItems = computed(() => {
    return this.filterMenuItems(this.allMenuItems);
  });

  private filterMenuItems(items: MenuItem[]): MenuItem[] {
    return items
      .filter(item => {
        if (!item.roles) return true;
        const hasRole = item.roles.some(role => this.authService.hasRole(role));
        if (!hasRole) {
          // console.log(`Filtering out ${item.label} because user misses roles: ${item.roles}`);
        }
        return hasRole;
      })
      .map(item => {
        if (item.children) {
          return { ...item, children: this.filterMenuItems(item.children) };
        }
        return item;
      })
      .filter(item => {
        // If item has children array but it's empty after filtering, hide the parent
        // because in this sidebar, parents with children are just toggles, not links.
        if (item.children && item.children.length === 0) {
          return false;
        }
        return true;
      });
  }

  toggleCollapse() {
    this.collapsed.set(!this.collapsed());
  }

  toggleMenu(label: string) {
    if (this.activeMenu() === label) {
      this.activeMenu.set(null);
    } else {
      this.activeMenu.set(label);
    }
  }
}

