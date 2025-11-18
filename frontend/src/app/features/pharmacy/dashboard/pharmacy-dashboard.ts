import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChartModule } from 'primeng/chart';
import { CardModule } from 'primeng/card';

@Component({
  selector: 'app-pharmacy-dashboard',
  standalone: true,
  imports: [CommonModule, ChartModule, CardModule],
  templateUrl: './pharmacy-dashboard.html',
  styleUrl: './pharmacy-dashboard.css'
})
export class PharmacyDashboardComponent {
  stats = [
    { label: 'Total Medications', value: '245', icon: 'pi pi-box', color: 'bg-blue-500' },
    { label: 'Low Stock Items', value: '12', icon: 'pi pi-exclamation-triangle', color: 'bg-yellow-500' },
    { label: 'Expiring Soon', value: '8', icon: 'pi pi-calendar-times', color: 'bg-red-500' },
    { label: 'Total Value', value: '$45,230', icon: 'pi pi-dollar', color: 'bg-green-500' }
  ];

  medicationChartData: any;
  chartOptions: any;

  constructor() {
    this.medicationChartData = {
      labels: ['Cardiology', 'Antibiotics', 'Pain Relief', 'Vitamins', 'Others'],
      datasets: [{
        data: [30, 25, 20, 15, 10],
        backgroundColor: ['#0ea5e9', '#10b981', '#f59e0b', '#ef4444', '#8b5cf6']
      }]
    };

    this.chartOptions = {
      maintainAspectRatio: false,
      aspectRatio: 0.6
    };
  }
}

