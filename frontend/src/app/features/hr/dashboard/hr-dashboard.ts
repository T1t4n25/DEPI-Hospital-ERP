import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChartModule } from 'primeng/chart';
import { CardModule } from 'primeng/card';

@Component({
  selector: 'app-hr-dashboard',
  standalone: true,
  imports: [CommonModule, ChartModule, CardModule],
  templateUrl: './hr-dashboard.html',
  styleUrl: './hr-dashboard.css'
})
export class HrDashboardComponent {
  stats = [
    { label: 'Total Employees', value: '89', icon: 'pi pi-users', color: 'bg-blue-500' },
    { label: 'Present Today', value: '75', icon: 'pi pi-check-circle', color: 'bg-green-500' },
    { label: 'On Leave', value: '8', icon: 'pi pi-calendar-times', color: 'bg-yellow-500' },
    { label: 'Departments', value: '12', icon: 'pi pi-building', color: 'bg-purple-500' }
  ];

  attendanceChartData: any;
  departmentChartData: any;
  chartOptions: any;

  constructor() {
    this.initCharts();
  }

  initCharts() {
    this.attendanceChartData = {
      labels: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
      datasets: [{
        label: 'Attendance',
        data: [75, 78, 80, 72, 76, 45, 30],
        backgroundColor: '#0ea5e9'
      }]
    };

    this.departmentChartData = {
      labels: ['Cardiology', 'Pediatrics', 'Emergency', 'Orthopedics', 'Others'],
      datasets: [{
        data: [25, 20, 18, 15, 11],
        backgroundColor: ['#0ea5e9', '#10b981', '#f59e0b', '#ef4444', '#8b5cf6']
      }]
    };

    this.chartOptions = {
      maintainAspectRatio: false,
      aspectRatio: 0.6
    };
  }
}

