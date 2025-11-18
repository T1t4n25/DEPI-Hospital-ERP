import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { SelectModule } from 'primeng/select';
import { InputNumberModule } from 'primeng/inputnumber';
import { TableModule } from 'primeng/table';
import { RippleModule } from 'primeng/ripple';

@Component({
  selector: 'app-invoice-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, CardModule, ButtonModule, SelectModule, InputNumberModule, TableModule, RippleModule],
  templateUrl: './invoice-form.html',
  styleUrl: './invoice-form.css'
})
export class InvoiceFormComponent {
  invoice = {
    patient: null as any,
    invoiceType: null as any,
    items: [] as any[]
  };

  patients = [
    { label: 'John Doe', value: 1 },
    { label: 'Jane Smith', value: 2 }
  ];

  invoiceTypes = [
    { label: 'Service', value: 'Service' },
    { label: 'Medication', value: 'Medication' }
  ];

  onSubmit() {
    console.log('Form submitted:', this.invoice);
  }
}

