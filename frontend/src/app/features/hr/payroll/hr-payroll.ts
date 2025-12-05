import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChartModule } from 'primeng/chart';
import { CardModule } from 'primeng/card';
import { TableModule } from 'primeng/table';
import { TagModule } from 'primeng/tag';

@Component({
  selector: 'app-hr-payroll',
  standalone: true,
  imports: [CommonModule, ChartModule, CardModule, TableModule, TagModule],
  templateUrl: './hr-payroll.html',
  styleUrl: './hr-payroll.css'
})
export class HrPayrollComponent {
  // TODO: This component currently uses static data as there is no backend API for Payroll yet.
  payrollData = [
    { id: 1, employee: 'Dr. John Smith', salary: '$8,000', deductions: '$1,200', net: '$6,800', status: 'Paid' },
    { id: 2, employee: 'Mary Williams', salary: '$5,000', deductions: '$750', net: '$4,250', status: 'Paid' }
  ];

  payrollChartData: any;
  chartOptions: any;

  constructor() {
    this.payrollChartData = {
      labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun'],
      datasets: [{
        label: 'Total Payroll',
        data: [450000, 480000, 460000, 490000, 470000, 500000],
        backgroundColor: '#10b981'
      }]
    };

    this.chartOptions = {
      maintainAspectRatio: false,
      aspectRatio: 0.6
    };
  }
}

