import { Component, DestroyRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChartModule } from 'primeng/chart';
import { CardModule } from 'primeng/card';
import { EmployeesService } from '../../employees/services/employees.service';
import { DepartmentsService } from '../../departments/services/departments.service';
import { forkJoin } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-hr-dashboard',
  standalone: true,
  imports: [CommonModule, ChartModule, CardModule],
  templateUrl: './hr-dashboard.html',
  styleUrl: './hr-dashboard.css'
})
export class HrDashboardComponent {
  stats = [
    { label: 'Total Employees', value: '...', icon: 'pi pi-users', color: 'bg-blue-500' },
    { label: 'Present Today', value: '...', icon: 'pi pi-check-circle', color: 'bg-green-500' },
    { label: 'On Leave', value: '...', icon: 'pi pi-calendar-times', color: 'bg-yellow-500' },
    { label: 'Departments', value: '...', icon: 'pi pi-building', color: 'bg-purple-500' }
  ];

  attendanceChartData: any;
  departmentChartData: any;
  chartOptions: any;

  constructor(
    private employeesService: EmployeesService,
    private departmentsService: DepartmentsService,
    private destroyRef: DestroyRef
  ) {
    this.initCharts();
    this.loadDashboardData();
  }

  initCharts() {
    this.attendanceChartData = {
      labels: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
      datasets: [{
        label: 'Attendance',
        data: [75, 78, 80, 72, 76, 45, 30], // Placeholder data
        backgroundColor: '#0ea5e9'
      }]
    };

    // Initial placeholder for department chart
    this.departmentChartData = {
      labels: [],
      datasets: [{
        data: [],
        backgroundColor: ['#0ea5e9', '#10b981', '#f59e0b', '#ef4444', '#8b5cf6', '#6366f1', '#ec4899']
      }]
    };

    this.chartOptions = {
      maintainAspectRatio: false,
      aspectRatio: 0.6
    };
  }

  private loadDashboardData() {
    forkJoin({
      employees: this.employeesService.getAll({ pageSize: 10000 }),
      departments: this.departmentsService.getAll({ pageSize: 10000 })
    }).pipe(
      takeUntilDestroyed(this.destroyRef)
    ).subscribe({
      next: ({ employees, departments }) => {
        this.calculateStats(employees.data, departments.data);
      },
      error: (err) => console.error('Failed to load dashboard data', err)
    });
  }

  private calculateStats(employees: any[], departments: any[]) {
    // 1. Total Employees
    const totalEmployees = employees.length;

    // 2. Departments Count
    const totalDepartments = departments.length;

    // 3. Placeholders for attendance (since we don't have real attendance data yet)
    const presentToday = Math.floor(totalEmployees * 0.9); // Assume 90% present
    const onLeave = totalEmployees - presentToday;

    this.stats = [
      { label: 'Total Employees', value: totalEmployees.toString(), icon: 'pi pi-users', color: 'bg-blue-500' },
      { label: 'Present Today', value: presentToday.toString(), icon: 'pi pi-check-circle', color: 'bg-green-500' },
      { label: 'On Leave', value: onLeave.toString(), icon: 'pi pi-calendar-times', color: 'bg-yellow-500' },
      { label: 'Departments', value: totalDepartments.toString(), icon: 'pi pi-building', color: 'bg-purple-500' }
    ];

    // 4. Department Distribution Chart
    const deptCounts: { [key: string]: number } = {};
    employees.forEach(emp => {
      const deptName = emp.departmentName || 'Unknown';
      deptCounts[deptName] = (deptCounts[deptName] || 0) + 1;
    });

    this.departmentChartData = {
      labels: Object.keys(deptCounts),
      datasets: [{
        data: Object.values(deptCounts),
        backgroundColor: ['#0ea5e9', '#10b981', '#f59e0b', '#ef4444', '#8b5cf6', '#6366f1', '#ec4899']
      }]
    };
  }
}

