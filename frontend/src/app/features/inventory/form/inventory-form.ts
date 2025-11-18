import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { SelectModule } from 'primeng/select';
import { InputNumberModule } from 'primeng/inputnumber';
import { DatePickerModule } from 'primeng/datepicker';
import { RippleModule } from 'primeng/ripple';

@Component({
  selector: 'app-inventory-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, CardModule, ButtonModule, SelectModule, InputNumberModule, DatePickerModule, RippleModule],
  templateUrl: './inventory-form.html',
  styleUrl: './inventory-form.css'
})
export class InventoryFormComponent {
  inventory = {
    medication: null as any,
    quantity: null as number | null,
    expiryDate: null as Date | null
  };

  medications = [
    { label: 'Aspirin', value: 1 },
    { label: 'Paracetamol', value: 2 }
  ];

  onSubmit() {
    console.log('Form submitted:', this.inventory);
  }
}

