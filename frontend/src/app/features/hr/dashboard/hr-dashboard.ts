import { Component, DestroyRef, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChartModule } from 'primeng/chart';
import { CardModule } from 'primeng/card';
import { DashboardService } from '../../dashboard/services/dashboard.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { HrDashboardDto } from '../../dashboard/models/dashboard.models';

@Component({
  selector: 'app-hr-dashboard',
  standalone: true,
  imports: [CommonModule, ChartModule, CardModule],
  templateUrl: './hr-dashboard.html',
  styleUrl: './hr-dashboard.css'
})
export class HrDashboardComponent {
  private dashboardService = inject(DashboardService);
  private destroyRef = inject(DestroyRef);

  loading = signal(true);
  dashboardData = signal<HrDashboardDto | null>(null);

  stats = signal([
    { label: 'Total Employees', value: '...', icon: 'pi pi-users', color: 'bg-blue-500' },
    { label: 'Present Today', value: '...', icon: 'pi pi-check-circle', color: 'bg-green-500' },
    { label: 'On Leave', value: '...', icon: 'pi pi-calendar-times', color: 'bg-yellow-500' },
    { label: 'Departments', value: '...', icon: 'pi pi-building', color: 'bg-purple-500' }
  ]);

  attendanceChartData: any;
  departmentChartData: any;
  roleChartData: any;
  chartOptions: any;

  recentHires = signal<any[]>([]);

  constructor() {
    this.initCharts();
    this.loadDashboardData();
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

  private loadDashboardData() {
    this.dashboardService.getHrDashboard()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (data) => {
          this.dashboardData.set(data);
          this.updateStats(data);
          this.updateCharts(data);
          this.updateRecentHires(data);
          this.loading.set(false);
        },
        error: (err) => {
          console.error('Failed to load HR dashboard data', err);
          this.loading.set(false);
        }
      });
  }

  private updateStats(data: HrDashboardDto) {
    this.stats.set([
      { label: 'Total Employees', value: data.totalEmployees.toString(), icon: 'pi pi-users', color: 'bg-blue-500' },
      { label: 'Present Today', value: data.presentToday.toString(), icon: 'pi pi-check-circle', color: 'bg-green-500' },
      { label: 'On Leave', value: data.onLeave.toString(), icon: 'pi pi-calendar-times', color: 'bg-yellow-500' },
      { label: 'Departments', value: data.totalDepartments.toString(), icon: 'pi pi-building', color: 'bg-purple-500' }
    ]);
  }

  private updateCharts(data: HrDashboardDto) {
    // Department Distribution Chart
    this.departmentChartData = {
      labels: data.departmentEmployeeCounts.map(d => d.departmentName),
      datasets: [{
        data: data.departmentEmployeeCounts.map(d => d.employeeCount),
        backgroundColor: ['#0ea5e9', '#10b981', '#f59e0b', '#ef4444', '#8b5cf6', '#6366f1', '#ec4899']
      }]
    };

    // Role Distribution Chart
    this.roleChartData = {
      labels: data.roleCounts.map(r => r.roleName),
      datasets: [{
        data: data.roleCounts.map(r => r.count),
        backgroundColor: ['#0ea5e9', '#10b981', '#f59e0b', '#ef4444', '#8b5cf6', '#6366f1', '#ec4899']
      }]
    };
  }

  private updateRecentHires(data: HrDashboardDto) {
    const hires = data.recentHires.map(hire => ({
      ...hire,
      hireDateFormatted: new Date(hire.hireDate).toLocaleDateString()
    }));
    this.recentHires.set(hires);
  }
}

