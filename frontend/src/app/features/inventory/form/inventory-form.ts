import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { SelectModule } from 'primeng/select';
import { InputNumberModule } from 'primeng/inputnumber';
import { DatePickerModule } from 'primeng/datepicker';
import { RippleModule } from 'primeng/ripple';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { InventoryService } from '../services/inventory.service';
import { MedicationsService } from '../../pharmacy/services/medications.service';
import { CreateInventoryModel } from '../models/create-inventory.model';
import { UpdateInventoryModel } from '../models/update-inventory.model';

@Component({
  selector: 'app-inventory-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, CardModule, ButtonModule, SelectModule, InputNumberModule, DatePickerModule, RippleModule, ToastModule],
  providers: [MessageService],
  templateUrl: './inventory-form.html',
  styleUrl: './inventory-form.css'
})
export class InventoryFormComponent implements OnInit {
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private inventoryService = inject(InventoryService);
  private medicationsService = inject(MedicationsService);
  private messageService = inject(MessageService);

  isSubmitting = false;
  isEditMode = false;
  isLoading = false;
  inventoryId: number | null = null;

  inventory = {
    medication: null as number | null,
    quantity: null as number | null,
    expiryDate: null as Date | null
  };

  medications: any[] = [];

  ngOnInit() {
    this.loadDropdownData();

    this.route.params.subscribe(params => {
      if (params['id']) {
        this.isEditMode = true;
        this.inventoryId = +params['id'];
        this.loadInventoryData();
      }
    });
  }

  loadDropdownData() {
    this.medicationsService.getAll({ pageSize: 1000 }).subscribe({
      next: (response) => {
        this.medications = response.data.map((m: any) => ({
          label: m.name,
          value: m.medicationID
        }));
      },
      error: (err) => console.error('Error loading medications:', err)
    });
  }

  loadInventoryData() {
    if (!this.inventoryId) return;

    this.isLoading = true;
    this.inventoryService.getById(this.inventoryId).subscribe({
      next: (data) => {
        this.inventory = {
          medication: data.medicationID,
          quantity: data.quantity,
          expiryDate: new Date(data.expiryDate)
        };
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading inventory item:', error);
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to load inventory data' });
        this.isLoading = false;
      }
    });
  }

  onSubmit() {
    if (this.isSubmitting) return;

    // Validation
    if (!this.inventory.medication || !this.inventory.quantity || !this.inventory.expiryDate) {
      this.messageService.add({ severity: 'warn', summary: 'Validation', detail: 'Please fill in all required fields.' });
      return;
    }

    this.isSubmitting = true;

    // Format date to YYYY-MM-DD
    const formattedDate = this.inventory.expiryDate.toISOString().split('T')[0];

    if (this.isEditMode && this.inventoryId) {
      // Update existing item
      const model: UpdateInventoryModel = {
        medicationID: this.inventory.medication,
        quantity: this.inventory.quantity,
        expiryDate: formattedDate
      };

      this.inventoryService.update(this.inventoryId, model).subscribe({
        next: () => {
          this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Inventory item updated successfully' });
          setTimeout(() => this.router.navigate(['/inventory']), 1000);
        },
        error: (error) => {
          console.error('Error updating inventory item:', error);
          this.messageService.add({ severity: 'error', summary: 'Error', detail: error.message || 'Failed to update inventory item' });
          this.isSubmitting = false;
        }
      });
    } else {
      // Create new item
      const model: CreateInventoryModel = {
        medicationID: this.inventory.medication,
        quantity: this.inventory.quantity,
        expiryDate: formattedDate
      };

      this.inventoryService.create(model).subscribe({
        next: () => {
          this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Inventory item created successfully' });
          setTimeout(() => this.router.navigate(['/inventory']), 1000);
        },
        error: (error) => {
          console.error('Error creating inventory item:', error);
          this.messageService.add({ severity: 'error', summary: 'Error', detail: error.message || 'Failed to create inventory item' });
          this.isSubmitting = false;
        }
      });
    }
  }
}

