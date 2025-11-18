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
  selector: 'app-medical-records-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, TableModule, ButtonModule, InputTextModule, TagModule, RippleModule],
  templateUrl: './medical-records-list.html',
  styleUrl: './medical-records-list.css'
})
export class MedicalRecordsListComponent {
  records = [
    { id: 1, patient: 'John Doe', doctor: 'Dr. Smith', diagnosis: 'Hypertension', date: '2024-01-15' },
    { id: 2, patient: 'Jane Smith', doctor: 'Dr. Johnson', diagnosis: 'Common Cold', date: '2024-01-10' },
    { id: 3, patient: 'Michael Johnson', doctor: 'Dr. Williams', diagnosis: 'Fracture', date: '2024-01-05' }
  ];

  searchTerm = '';

  get filteredRecords() {
    if (!this.searchTerm) return this.records;
    const term = this.searchTerm.toLowerCase();
    return this.records.filter(r => 
      r.patient.toLowerCase().includes(term) ||
      r.doctor.toLowerCase().includes(term) ||
      r.diagnosis.toLowerCase().includes(term)
    );
  }
}

