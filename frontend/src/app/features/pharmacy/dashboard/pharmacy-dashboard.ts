import { Component, DestroyRef, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChartModule } from 'primeng/chart';
import { CardModule } from 'primeng/card';
import { DashboardService } from '../../dashboard/services/dashboard.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-pharmacy-dashboard',
  standalone: true,
  imports: [CommonModule, ChartModule, CardModule],
  templateUrl: './pharmacy-dashboard.html',
  styleUrl: './pharmacy-dashboard.css'
})
export class PharmacyDashboardComponent {
  private dashboardService = inject(DashboardService);
  private destroyRef = inject(DestroyRef);

  loading = signal(true);
  dashboardData = signal<any>(null);

  stats = signal([
    { label: 'Total Medications', value: '...', icon: 'pi pi-box', color: 'bg-blue-500' },
    { label: 'Low Stock Items', value: '...', icon: 'pi pi-exclamation-triangle', color: 'bg-yellow-500' },
    { label: 'Expired Items', value: '...', icon: 'pi pi-times-circle', color: 'bg-red-500' },
    { label: 'Expiring Soon', value: '...', icon: 'pi pi-calendar-times', color: 'bg-orange-500' },
    { label: 'Total Value', value: '...', icon: 'pi pi-dollar', color: 'bg-green-500' }
  ]);

  medicationChartData: any;
  chartOptions: any;

  constructor() {
    this.initChart();
    this.loadDashboardData();
  }

  private initChart() {
    this.medicationChartData = {
      labels: ['Cardiology', 'Antibiotics', 'Pain Relief', 'Vitamins', 'Others'],
      datasets: [{
        data: [30, 25, 20, 15, 10], // Placeholder for now as we don't have category data
        backgroundColor: ['#0ea5e9', '#10b981', '#f59e0b', '#ef4444', '#8b5cf6']
      }]
    };

    this.chartOptions = {
      maintainAspectRatio: false,
      aspectRatio: 0.6
    };
  }

  private loadDashboardData() {
    this.dashboardService.getPharmacyDashboard()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (data) => {
          this.dashboardData.set(data);
          this.updateStats(data);
          this.loading.set(false);
        },
        error: (err) => {
          console.error('Failed to load pharmacy dashboard data', err);
          this.loading.set(false);
        }
      });
  }

  private updateStats(data: any) {
    const formatCurrency = (value: number): string => {
      return new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD',
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
      }).format(value);
    };

    this.stats.set([
      { label: 'Total Medications', value: data.totalMedications.toString(), icon: 'pi pi-box', color: 'bg-blue-500' },
      { label: 'Low Stock Items', value: data.lowStockItems.toString(), icon: 'pi pi-exclamation-triangle', color: 'bg-yellow-500' },
      { label: 'Expired Items', value: data.expiredItems.toString(), icon: 'pi pi-times-circle', color: 'bg-red-500' },
      { label: 'Expiring Soon', value: data.expiringSoonItems.toString(), icon: 'pi pi-calendar-times', color: 'bg-orange-500' },
      { label: 'Total Value', value: formatCurrency(data.totalValue), icon: 'pi pi-dollar', color: 'bg-green-500' }
    ]);
  }
}

