import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TableModule } from 'primeng/table';
import { InputTextModule } from 'primeng/inputtext';
import { TagModule } from 'primeng/tag';
import { RippleModule } from 'primeng/ripple';

@Component({
  selector: 'app-pharmacy-medications',
  standalone: true,
  imports: [CommonModule, FormsModule, TableModule, InputTextModule, TagModule, RippleModule],
  templateUrl: './pharmacy-medications.html',
  styleUrl: './pharmacy-medications.css'
})
export class PharmacyMedicationsComponent {
  medications = [
    { id: 1, name: 'Aspirin', barcode: '123456789', cost: '$5.00', stock: 150, status: 'In Stock' },
    { id: 2, name: 'Paracetamol', barcode: '987654321', cost: '$3.50', stock: 45, status: 'Low Stock' }
  ];

  searchTerm = '';
}

