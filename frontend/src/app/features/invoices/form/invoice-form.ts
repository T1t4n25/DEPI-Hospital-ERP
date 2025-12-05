import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { SelectModule } from 'primeng/select';
import { InputNumberModule } from 'primeng/inputnumber';
import { TableModule } from 'primeng/table';
import { RippleModule } from 'primeng/ripple';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { InvoicesService } from '../services/invoices.service';
import { PatientsService } from '../../patients/services/patients.service';
import { CreateInvoiceModel } from '../models/create-invoice.model';
import { UpdateInvoiceModel } from '../models/update-invoice.model';

@Component({
  selector: 'app-invoice-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, CardModule, ButtonModule, SelectModule, InputNumberModule, TableModule, RippleModule, ToastModule],
  providers: [MessageService],
  templateUrl: './invoice-form.html',
  styleUrl: './invoice-form.css'
})
export class InvoiceFormComponent implements OnInit {
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private invoicesService = inject(InvoicesService);
  private patientsService = inject(PatientsService);
  private messageService = inject(MessageService);

  isSubmitting = false;
  isEditMode = false;
  isLoading = false;
  invoiceId: number | null = null;

  invoice = {
    patient: null as number | null,
    invoiceType: null as number | null,
    items: [] as any[]
  };

  patients: any[] = [];

  invoiceTypes = [
    { label: 'Hospital Service', value: 1 },
    { label: 'Pharmacy Medication', value: 2 }
  ];

  ngOnInit() {
    this.loadDropdownData();

    this.route.params.subscribe(params => {
      if (params['id']) {
        this.isEditMode = true;
        this.invoiceId = +params['id'];
        this.loadInvoiceData();
      }
    });
  }

  loadDropdownData() {
    this.patientsService.getAll({ pageSize: 1000 }).subscribe({
      next: (response) => {
        this.patients = response.data.map((p: any) => ({
          label: `${p.firstName} ${p.lastName}`,
          value: p.patientID
        }));
      },
      error: (err) => console.error('Error loading patients:', err)
    });
  }

  loadInvoiceData() {
    if (!this.invoiceId) return;

    this.isLoading = true;
    this.invoicesService.getById(this.invoiceId).subscribe({
      next: (data) => {
        this.invoice = {
          patient: data.patientID,
          invoiceType: data.invoiceTypeID,
          items: [...data.hospitalItems, ...data.medicationItems]
        };
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading invoice:', error);
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to load invoice data' });
        this.isLoading = false;
      }
    });
  }

  onSubmit() {
    if (this.isSubmitting) return;

    // Validation
    if (!this.invoice.patient || !this.invoice.invoiceType) {
      this.messageService.add({ severity: 'warn', summary: 'Validation', detail: 'Please fill in all required fields.' });
      return;
    }

    this.isSubmitting = true;

    if (this.isEditMode && this.invoiceId) {
      // Update existing invoice (Payment Status only for now as per service)
      // For now, we'll just show a message that update is limited or implement what we can
      // But since the form doesn't expose payment status, we might be limited.
      // Let's just try to update what we can or leave it as is.
      // Actually, the service only has updatePaymentStatus.
      // So general update might not be supported via API yet.
      this.messageService.add({ severity: 'info', summary: 'Info', detail: 'Update functionality is limited to payment status.' });
      this.isSubmitting = false;
    } else {
      // Create new invoice
      const model: CreateInvoiceModel = {
        patientID: this.invoice.patient,
        invoiceTypeID: this.invoice.invoiceType,
        invoiceDate: new Date().toISOString().split('T')[0],
        paymentStatusID: 2, // Default to Unpaid
        hospitalItems: [], // Empty for now
        medicationItems: [] // Empty for now
      };

      this.invoicesService.create(model).subscribe({
        next: () => {
          this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Invoice created successfully' });
          setTimeout(() => this.router.navigate(['/invoices']), 1000);
        },
        error: (error) => {
          console.error('Error creating invoice:', error);
          this.messageService.add({ severity: 'error', summary: 'Error', detail: error.message || 'Failed to create invoice' });
          this.isSubmitting = false;
        }
      });
    }
  }
}

