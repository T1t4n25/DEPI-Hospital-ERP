import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TableModule } from 'primeng/table';
import { InputTextModule } from 'primeng/inputtext';
import { TagModule } from 'primeng/tag';
import { RippleModule } from 'primeng/ripple';

@Component({
  selector: 'app-pharmacy-prescriptions',
  standalone: true,
  imports: [CommonModule, FormsModule, TableModule, InputTextModule, TagModule, RippleModule],
  templateUrl: './pharmacy-prescriptions.html',
  styleUrl: './pharmacy-prescriptions.css'
})
export class PharmacyPrescriptionsComponent {
  // TODO: This component currently uses static data as there is no backend API for Prescriptions yet.
  prescriptions = [
    { id: 1, patient: 'John Doe', medication: 'Aspirin', quantity: 30, date: '2024-01-15', status: 'Filled' },
    { id: 2, patient: 'Jane Smith', medication: 'Paracetamol', quantity: 20, date: '2024-01-16', status: 'Pending' }
  ];

  searchTerm = '';
}

