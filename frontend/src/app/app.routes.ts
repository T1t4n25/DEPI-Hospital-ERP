import { Routes } from '@angular/router';
import { MainLayoutComponent } from './core/layouts/main-layout/main-layout';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () => import('./core/auth/login/login.component').then(m => m.LoginComponent)
  },
  {
    path: 'auth/callback',
    loadComponent: () => import('./core/auth/callback/auth-callback.component').then(m => m.AuthCallbackComponent)
  },
  {
    path: '',
    component: MainLayoutComponent,
    canActivate: [authGuard],
    children: [
      {
        path: '',
        redirectTo: '/dashboard',
        pathMatch: 'full'
      },
      {
        path: 'dashboard',
        loadComponent: () => import('./features/dashboard/dashboard').then(m => m.DashboardComponent)
      },
      // Patients routes
      {
        path: 'patients',
        loadComponent: () => import('./features/patients/list/patients-list').then(m => m.PatientsListComponent)
      },
      {
        path: 'patients/new',
        loadComponent: () => import('./features/patients/form/patient-form').then(m => m.PatientFormComponent)
      },
      {
        path: 'patients/:id',
        loadComponent: () => import('./features/patients/detail/patient-detail').then(m => m.PatientDetailComponent)
      },
      // Appointments routes
      {
        path: 'appointments',
        loadComponent: () => import('./features/appointments/list/appointments-list').then(m => m.AppointmentsListComponent)
      },
      {
        path: 'appointments/calendar',
        loadComponent: () => import('./features/appointments/calendar/appointments-calendar').then(m => m.AppointmentsCalendarComponent)
      },
      {
        path: 'appointments/new',
        loadComponent: () => import('./features/appointments/form/appointment-form').then(m => m.AppointmentFormComponent)
      },
      // Medical Records routes
      {
        path: 'medical-records',
        loadComponent: () => import('./features/medical-records/list/medical-records-list').then(m => m.MedicalRecordsListComponent)
      },
      {
        path: 'medical-records/new',
        loadComponent: () => import('./features/medical-records/form/medical-record-form').then(m => m.MedicalRecordFormComponent)
      },
      {
        path: 'medical-records/:id',
        loadComponent: () => import('./features/medical-records/detail/medical-record-detail').then(m => m.MedicalRecordDetailComponent)
      },
      // Employees routes
      {
        path: 'employees',
        loadComponent: () => import('./features/employees/list/employees-list').then(m => m.EmployeesListComponent)
      },
      {
        path: 'employees/new',
        loadComponent: () => import('./features/employees/form/employee-form').then(m => m.EmployeeFormComponent)
      },
      {
        path: 'employees/:id',
        loadComponent: () => import('./features/employees/detail/employee-detail').then(m => m.EmployeeDetailComponent)
      },
      // Departments routes
      {
        path: 'departments',
        loadComponent: () => import('./features/departments/list/departments-list').then(m => m.DepartmentsListComponent)
      },
      {
        path: 'departments/new',
        loadComponent: () => import('./features/departments/form/department-form').then(m => m.DepartmentFormComponent)
      },
      // HR routes
      {
        path: 'hr/dashboard',
        loadComponent: () => import('./features/hr/dashboard/hr-dashboard').then(m => m.HrDashboardComponent)
      },
      {
        path: 'hr/employees',
        loadComponent: () => import('./features/hr/employees/hr-employees').then(m => m.HrEmployeesComponent)
      },
      {
        path: 'hr/attendance',
        loadComponent: () => import('./features/hr/attendance/hr-attendance').then(m => m.HrAttendanceComponent)
      },
      {
        path: 'hr/payroll',
        loadComponent: () => import('./features/hr/payroll/hr-payroll').then(m => m.HrPayrollComponent)
      },
      // Pharmacy routes
      {
        path: 'pharmacy/dashboard',
        loadComponent: () => import('./features/pharmacy/dashboard/pharmacy-dashboard').then(m => m.PharmacyDashboardComponent)
      },
      {
        path: 'pharmacy/medications',
        loadComponent: () => import('./features/pharmacy/medications/pharmacy-medications').then(m => m.PharmacyMedicationsComponent)
      },
      {
        path: 'pharmacy/inventory',
        loadComponent: () => import('./features/pharmacy/inventory/pharmacy-inventory').then(m => m.PharmacyInventoryComponent)
      },
      {
        path: 'pharmacy/prescriptions',
        loadComponent: () => import('./features/pharmacy/prescriptions/pharmacy-prescriptions').then(m => m.PharmacyPrescriptionsComponent)
      },
      // Services routes
      {
        path: 'services',
        loadComponent: () => import('./features/services/list/services-list').then(m => m.ServicesListComponent)
      },
      {
        path: 'services/new',
        loadComponent: () => import('./features/services/form/service-form').then(m => m.ServiceFormComponent)
      },
      // Invoices routes
      {
        path: 'invoices',
        loadComponent: () => import('./features/invoices/list/invoices-list').then(m => m.InvoicesListComponent)
      },
      {
        path: 'invoices/new',
        loadComponent: () => import('./features/invoices/form/invoice-form').then(m => m.InvoiceFormComponent)
      },
      {
        path: 'invoices/:id',
        loadComponent: () => import('./features/invoices/detail/invoice-detail').then(m => m.InvoiceDetailComponent)
      },
      // Inventory routes
      {
        path: 'inventory',
        loadComponent: () => import('./features/inventory/list/inventory-list').then(m => m.InventoryListComponent)
      },
      {
        path: 'inventory/new',
        loadComponent: () => import('./features/inventory/form/inventory-form').then(m => m.InventoryFormComponent)
      }
    ]
  },
  // Static pages (no layout)
  {
    path: 'about-us',
    loadComponent: () => import('./features/static-pages/about-us/about-us').then(m => m.AboutUs)
  },
  {
    path: 'privacy-policy',
    loadComponent: () => import('./features/static-pages/privacy-policy/privacy-policy').then(m => m.PrivacyPolicy)
  },
  {
    path: 'maintainence',
    loadComponent: () => import('./features/static-pages/maintainence/maintainence').then(m => m.Maintainence)
  },
  {
    path: '**',
    loadComponent: () => import('./features/static-pages/not-found/not-found').then(m => m.NotFound)
  }
];
