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
  selector: 'app-inventory-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, TableModule, ButtonModule, InputTextModule, TagModule, RippleModule],
  templateUrl: './inventory-list.html',
  styleUrl: './inventory-list.css'
})
export class InventoryListComponent {
  inventory = [
    { id: 1, medication: 'Aspirin', quantity: 150, expiryDate: '2025-12-31', status: 'Good' },
    { id: 2, medication: 'Paracetamol', quantity: 45, expiryDate: '2024-06-30', status: 'Expiring Soon' }
  ];

  searchTerm = '';
}

