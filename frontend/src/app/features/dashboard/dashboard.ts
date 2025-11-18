import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChartModule } from 'primeng/chart';
import { CardModule } from 'primeng/card';
import { TableModule } from 'primeng/table';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, ChartModule, CardModule, TableModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class DashboardComponent {
  // Statistics data
  stats = [
    { label: 'Total Patients', value: '1,234', icon: 'pi pi-users', color: 'bg-blue-500', change: '+12%' },
    { label: 'Today\'s Appointments', value: '45', icon: 'pi pi-calendar', color: 'bg-green-500', change: '+5%' },
    { label: 'Revenue', value: '$125,430', icon: 'pi pi-dollar', color: 'bg-yellow-500', change: '+18%' },
    { label: 'Active Employees', value: '89', icon: 'pi pi-id-card', color: 'bg-purple-500', change: '+3%' }
  ];

  // Chart data
  appointmentsChartData: any;
  revenueChartData: any;
  departmentChartData: any;

  chartOptions: any;

  // Recent activities
  recentActivities = [
    { id: 1, type: 'Appointment', description: 'New appointment scheduled for John Doe', time: '5 min ago', icon: 'pi pi-calendar', color: 'text-blue-500' },
    { id: 2, type: 'Patient', description: 'New patient registered: Jane Smith', time: '15 min ago', icon: 'pi pi-user-plus', color: 'text-green-500' },
    { id: 3, type: 'Invoice', description: 'Invoice #1234 created', time: '1 hour ago', icon: 'pi pi-file', color: 'text-yellow-500' },
    { id: 4, type: 'Medical Record', description: 'Medical record updated for Patient #567', time: '2 hours ago', icon: 'pi pi-file-medical', color: 'text-purple-500' }
  ];

  constructor() {
    this.initCharts();
  }

  initCharts() {
    const documentStyle = getComputedStyle(document.documentElement);
    const textColor = documentStyle.getPropertyValue('--text-color');
    const textColorSecondary = documentStyle.getPropertyValue('--text-color-secondary');
    const surfaceBorder = documentStyle.getPropertyValue('--surface-border');

    // Appointments Chart (Line Chart)
    this.appointmentsChartData = {
      labels: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
      datasets: [
        {
          label: 'Appointments',
          data: [12, 19, 15, 25, 22, 18, 14],
          fill: false,
          borderColor: '#0ea5e9',
          tension: 0.4
        }
      ]
    };

    // Revenue Chart (Bar Chart)
    this.revenueChartData = {
      labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun'],
      datasets: [
        {
          label: 'Revenue',
          data: [45000, 52000, 48000, 61000, 55000, 65000],
          backgroundColor: '#10b981'
        }
      ]
    };

    // Department Chart (Pie Chart)
    this.departmentChartData = {
      labels: ['Cardiology', 'Pediatrics', 'Orthopedics', 'Neurology', 'Emergency'],
      datasets: [
        {
          data: [25, 20, 18, 15, 22],
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
}

