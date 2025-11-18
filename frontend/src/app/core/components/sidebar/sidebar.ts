import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

interface MenuItem {
  label: string;
  icon: string;
  route: string;
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
  collapsed = signal(false);
  activeMenu = signal<string | null>(null);

  menuItems: MenuItem[] = [
    {
      label: 'Dashboard',
      icon: 'pi pi-home',
      route: '/dashboard'
    },
    {
      label: 'Patients',
      icon: 'pi pi-users',
      route: '/patients',
      children: [
        { label: 'All Patients', icon: 'pi pi-list', route: '/patients' },
        { label: 'Add Patient', icon: 'pi pi-plus', route: '/patients/new' }
      ]
    },
    {
      label: 'Appointments',
      icon: 'pi pi-calendar',
      route: '/appointments',
      children: [
        { label: 'All Appointments', icon: 'pi pi-list', route: '/appointments' },
        { label: 'Calendar View', icon: 'pi pi-calendar', route: '/appointments/calendar' },
        { label: 'New Appointment', icon: 'pi pi-plus', route: '/appointments/new' }
      ]
    },
    {
      label: 'Medical Records',
      icon: 'pi pi-file-medical',
      route: '/medical-records',
      children: [
        { label: 'All Records', icon: 'pi pi-list', route: '/medical-records' },
        { label: 'New Record', icon: 'pi pi-plus', route: '/medical-records/new' }
      ]
    },
    {
      label: 'Employees',
      icon: 'pi pi-id-card',
      route: '/employees',
      children: [
        { label: 'All Employees', icon: 'pi pi-list', route: '/employees' },
        { label: 'Add Employee', icon: 'pi pi-plus', route: '/employees/new' }
      ]
    },
    {
      label: 'Departments',
      icon: 'pi pi-building',
      route: '/departments',
      children: [
        { label: 'All Departments', icon: 'pi pi-list', route: '/departments' },
        { label: 'Add Department', icon: 'pi pi-plus', route: '/departments/new' }
      ]
    },
    {
      label: 'HR',
      icon: 'pi pi-briefcase',
      route: '/hr',
      children: [
        { label: 'HR Dashboard', icon: 'pi pi-chart-bar', route: '/hr/dashboard' },
        { label: 'Employees', icon: 'pi pi-users', route: '/hr/employees' },
        { label: 'Attendance', icon: 'pi pi-clock', route: '/hr/attendance' },
        { label: 'Payroll', icon: 'pi pi-dollar', route: '/hr/payroll' }
      ]
    },
    {
      label: 'Pharmacy',
      icon: 'pi pi-shopping-cart',
      route: '/pharmacy',
      children: [
        { label: 'Pharmacy Dashboard', icon: 'pi pi-chart-bar', route: '/pharmacy/dashboard' },
        { label: 'Medications', icon: 'pi pi-box', route: '/pharmacy/medications' },
        { label: 'Inventory', icon: 'pi pi-warehouse', route: '/pharmacy/inventory' },
        { label: 'Prescriptions', icon: 'pi pi-file', route: '/pharmacy/prescriptions' }
      ]
    },
    {
      label: 'Services',
      icon: 'pi pi-cog',
      route: '/services',
      children: [
        { label: 'All Services', icon: 'pi pi-list', route: '/services' },
        { label: 'Add Service', icon: 'pi pi-plus', route: '/services/new' }
      ]
    },
    {
      label: 'Invoices',
      icon: 'pi pi-file-edit',
      route: '/invoices',
      children: [
        { label: 'All Invoices', icon: 'pi pi-list', route: '/invoices' },
        { label: 'New Invoice', icon: 'pi pi-plus', route: '/invoices/new' }
      ]
    },
    {
      label: 'Inventory',
      icon: 'pi pi-warehouse',
      route: '/inventory',
      children: [
        { label: 'All Inventory', icon: 'pi pi-list', route: '/inventory' },
        { label: 'Add Item', icon: 'pi pi-plus', route: '/inventory/new' }
      ]
    }
  ];

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

