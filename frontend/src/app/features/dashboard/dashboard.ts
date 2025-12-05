import { Component, DestroyRef, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChartModule } from 'primeng/chart';
import { CardModule } from 'primeng/card';
import { TableModule } from 'primeng/table';
import { DashboardService } from './services/dashboard.service';
import { AuthService } from '../../core/services/auth.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { AdminDashboardDto } from './models/dashboard.models';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, ChartModule, CardModule, TableModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class DashboardComponent {
  private dashboardService = inject(DashboardService);
  private authService = inject(AuthService);
  private destroyRef = inject(DestroyRef);

  loading = signal(true);
  dashboardData = signal<AdminDashboardDto | null>(null);
  accountantData = signal<any | null>(null);

  // Statistics data
  stats = signal([
    { label: 'Total Patients', value: '...', icon: 'pi pi-users', color: 'bg-blue-500' },
    { label: 'Today\'s Appointments', value: '...', icon: 'pi pi-calendar', color: 'bg-green-500' },
    { label: 'Revenue', value: '...', icon: 'pi pi-dollar', color: 'bg-yellow-500' },
    { label: 'Active Employees', value: '...', icon: 'pi pi-id-card', color: 'bg-purple-500' }
  ]);

  // Chart data
  appointmentsChartData: any;
  revenueChartData: any;
  departmentChartData: any;

  chartOptions: any;

  // Recent activities
  recentActivities = signal<any[]>([]);

  constructor() {
    this.initCharts();
    this.loadDashboardData();
  }

  loadDashboardData() {
    // Determine which dashboard to load based on user role
    const user = this.authService.currentUser();
    
    if (this.authService.hasRole('Admin')) {
      this.dashboardService.getAdminDashboard()
        .pipe(takeUntilDestroyed(this.destroyRef))
        .subscribe({
          next: (data) => {
            this.dashboardData.set(data);
            this.updateStats(data);
            this.updateCharts(data);
            this.updateRecentActivities(data);
            this.loading.set(false);
          },
          error: (err) => {
            console.error('Failed to load admin dashboard:', err);
            this.loading.set(false);
          }
        });
    } else if (this.authService.hasRole('Accountant')) {
      this.dashboardService.getAccountantDashboard()
        .pipe(takeUntilDestroyed(this.destroyRef))
        .subscribe({
          next: (data) => {
            this.accountantData.set(data);
            // For accountant, we'll show different stats
            this.stats.set([
              { label: 'Total Revenue', value: this.formatCurrency(data.totalRevenue), icon: 'pi pi-dollar', color: 'bg-green-500' },
              { label: 'Pending Payments', value: this.formatCurrency(data.pendingPayments), icon: 'pi pi-clock', color: 'bg-yellow-500' },
              { label: 'Total Invoices', value: data.totalInvoices.toString(), icon: 'pi pi-file', color: 'bg-blue-500' },
              { label: 'Paid Invoices', value: data.paidInvoices.toString(), icon: 'pi pi-check-circle', color: 'bg-purple-500' }
            ]);
            this.updateAccountantCharts(data);
            this.updateAccountantRecentInvoices(data);
            this.loading.set(false);
          },
          error: (err) => {
            console.error('Failed to load accountant dashboard:', err);
            this.loading.set(false);
          }
        });
    } else {
      // Default or other roles - show admin dashboard if accessible
      this.dashboardService.getAdminDashboard()
        .pipe(takeUntilDestroyed(this.destroyRef))
        .subscribe({
          next: (data) => {
            this.dashboardData.set(data);
            this.updateStats(data);
            this.updateCharts(data);
            this.updateRecentActivities(data);
            this.loading.set(false);
          },
          error: (err) => {
            console.error('Failed to load dashboard:', err);
            this.loading.set(false);
          }
        });
    }
  }

  updateStats(data: AdminDashboardDto) {
    this.stats.set([
      { label: 'Total Patients', value: data.totalPatients.toLocaleString(), icon: 'pi pi-users', color: 'bg-blue-500' },
      { label: 'Today\'s Appointments', value: data.todayAppointments.toString(), icon: 'pi pi-calendar', color: 'bg-green-500' },
      { label: 'Revenue', value: this.formatCurrency(data.totalRevenue), icon: 'pi pi-dollar', color: 'bg-yellow-500' },
      { label: 'Active Employees', value: data.activeEmployees.toString(), icon: 'pi pi-id-card', color: 'bg-purple-500' }
    ]);
  }

  updateCharts(data: AdminDashboardDto) {
    // Weekly Appointments Chart
    this.appointmentsChartData = {
      labels: data.weeklyAppointments.map(w => w.day),
      datasets: [
        {
          label: 'Appointments',
          data: data.weeklyAppointments.map(w => w.count),
          fill: false,
          borderColor: '#0ea5e9',
          tension: 0.4
        }
      ]
    };

    // Monthly Revenue Chart
    this.revenueChartData = {
      labels: data.monthlyRevenue.map(m => m.month),
      datasets: [
        {
          label: 'Revenue',
          data: data.monthlyRevenue.map(m => m.amount),
          backgroundColor: '#10b981'
        }
      ]
    };

    // Department Chart
    this.departmentChartData = {
      labels: data.departmentStats.map(d => d.departmentName),
      datasets: [
        {
          data: data.departmentStats.map(d => d.employeeCount),
          backgroundColor: [
            '#0ea5e9',
            '#10b981',
            '#f59e0b',
            '#ef4444',
            '#8b5cf6',
            '#6366f1',
            '#ec4899'
          ]
        }
      ]
    };
  }

  updateAccountantCharts(data: any) {
    // Monthly Revenue Chart
    this.revenueChartData = {
      labels: data.monthlyRevenue.map((m: any) => m.month),
      datasets: [
        {
          label: 'Revenue',
          data: data.monthlyRevenue.map((m: any) => m.amount),
          backgroundColor: '#10b981'
        }
      ]
    };

    // Invoice Status Breakdown
    this.departmentChartData = {
      labels: data.invoiceStatusBreakdown.map((s: any) => s.statusName),
      datasets: [
        {
          data: data.invoiceStatusBreakdown.map((s: any) => s.count),
          backgroundColor: [
            '#0ea5e9',
            '#10b981',
            '#f59e0b',
            '#ef4444',
            '#8b5cf6'
          ]
        }
      ]
    };
  }

  updateAccountantRecentInvoices(data: any) {
    const invoices = data.recentInvoices.map((invoice: any, index: number) => {
      const invoiceDate = new Date(invoice.invoiceDate);
      const timeAgo = this.getTimeAgo(invoiceDate);
      
      return {
        id: invoice.invoiceID,
        invoiceID: invoice.invoiceID,
        patientName: invoice.patientName,
        totalAmount: this.formatCurrency(invoice.totalAmount),
        paymentStatus: invoice.paymentStatusName,
        invoiceDate: invoiceDate.toLocaleDateString(),
        time: timeAgo,
        icon: 'pi pi-file',
        color: invoice.paymentStatusName === 'Paid' ? 'text-green-500' : 'text-yellow-500'
      };
    });

    this.recentActivities.set(invoices);
  }

  updateRecentActivities(data: AdminDashboardDto) {
    const activities = data.recentActivities.map((activity, index) => {
      const timeAgo = this.getTimeAgo(new Date(activity.timestamp));
      let icon = 'pi pi-info-circle';
      let color = 'text-gray-500';

      switch (activity.type) {
        case 'Appointment':
          icon = 'pi pi-calendar';
          color = 'text-blue-500';
          break;
        case 'Invoice':
          icon = 'pi pi-file';
          color = 'text-yellow-500';
          break;
        case 'Medical Record':
          icon = 'pi pi-file-medical';
          color = 'text-purple-500';
          break;
        case 'Patient':
          icon = 'pi pi-user-plus';
          color = 'text-green-500';
          break;
      }

      return {
        id: index + 1,
        type: activity.type,
        description: activity.description,
        time: timeAgo,
        icon,
        color
      };
    });

    this.recentActivities.set(activities);
  }

  initCharts() {
    const documentStyle = getComputedStyle(document.documentElement);
    const textColor = documentStyle.getPropertyValue('--text-color');
    const textColorSecondary = documentStyle.getPropertyValue('--text-color-secondary');
    const surfaceBorder = documentStyle.getPropertyValue('--surface-border');

    this.chartOptions = {
      maintainAspectRatio: false,
      aspectRatio: 0.6,
      plugins: {
        legend: {
          labels: {
            color: textColor
          }
        }
      },
      scales: {
        x: {
          ticks: {
            color: textColorSecondary
          },
          grid: {
            color: surfaceBorder
          }
        },
        y: {
          ticks: {
            color: textColorSecondary
          },
          grid: {
            color: surfaceBorder
          }
        }
      }
    };
  }

  formatCurrency(value: number): string {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD',
      minimumFractionDigits: 0,
      maximumFractionDigits: 0
    }).format(value);
  }

  getTimeAgo(date: Date): string {
    const now = new Date();
    const diffInSeconds = Math.floor((now.getTime() - date.getTime()) / 1000);

    if (diffInSeconds < 60) {
      return `${diffInSeconds} seconds ago`;
    } else if (diffInSeconds < 3600) {
      const minutes = Math.floor(diffInSeconds / 60);
      return `${minutes} ${minutes === 1 ? 'minute' : 'minutes'} ago`;
    } else if (diffInSeconds < 86400) {
      const hours = Math.floor(diffInSeconds / 3600);
      return `${hours} ${hours === 1 ? 'hour' : 'hours'} ago`;
    } else {
      const days = Math.floor(diffInSeconds / 86400);
      return `${days} ${days === 1 ? 'day' : 'days'} ago`;
    }
  }
}

