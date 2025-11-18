import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { TagModule } from 'primeng/tag';
import { RippleModule } from 'primeng/ripple';

@Component({
  selector: 'app-invoices-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, TableModule, ButtonModule, InputTextModule, TagModule, RippleModule],
  templateUrl: './invoices-list.html',
  styleUrl: './invoices-list.css'
})
export class InvoicesListComponent {
  invoices = [
    { id: 1, patient: 'John Doe', date: '2024-01-15', totalAmount: '$250', paymentStatus: 'Paid' },
    { id: 2, patient: 'Jane Smith', date: '2024-01-16', totalAmount: '$150', paymentStatus: 'Pending' }
  ];

  searchTerm = '';

  getStatusSeverity(status: string) {
    return status === 'Paid' ? 'success' : 'warn';
  }
}

