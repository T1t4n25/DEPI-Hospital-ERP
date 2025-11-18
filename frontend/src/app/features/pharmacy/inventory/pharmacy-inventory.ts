import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TableModule } from 'primeng/table';
import { InputTextModule } from 'primeng/inputtext';
import { TagModule } from 'primeng/tag';
import { RippleModule } from 'primeng/ripple';

@Component({
  selector: 'app-pharmacy-inventory',
  standalone: true,
  imports: [CommonModule, FormsModule, TableModule, InputTextModule, TagModule, RippleModule],
  templateUrl: './pharmacy-inventory.html',
  styleUrl: './pharmacy-inventory.css'
})
export class PharmacyInventoryComponent {
  inventory = [
    { id: 1, medication: 'Aspirin', quantity: 150, expiryDate: '2025-12-31', status: 'Good' },
    { id: 2, medication: 'Paracetamol', quantity: 45, expiryDate: '2024-06-30', status: 'Expiring Soon' }
  ];

  searchTerm = '';
}

