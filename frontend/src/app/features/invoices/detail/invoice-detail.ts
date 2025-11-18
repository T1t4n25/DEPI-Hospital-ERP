import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { TableModule } from 'primeng/table';

@Component({
  selector: 'app-invoice-detail',
  standalone: true,
  imports: [CommonModule, RouterModule, CardModule, ButtonModule, TagModule, TableModule],
  templateUrl: './invoice-detail.html',
  styleUrl: './invoice-detail.css'
})
export class InvoiceDetailComponent {
  invoice = {
    id: 1,
    patient: 'John Doe',
    date: '2024-01-15',
    totalAmount: '$250',
    paymentStatus: 'Paid'
  };

  items = [
    { service: 'General Checkup', quantity: 1, unitPrice: '$100', total: '$100' },
    { service: 'X-Ray', quantity: 1, unitPrice: '$150', total: '$150' }
  ];
}

